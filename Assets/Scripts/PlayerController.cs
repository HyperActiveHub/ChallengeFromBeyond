using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Animator anim;
    private Rigidbody2D rbody = null;
    private float distanceTravelled = 0;
    public float offseter = 0.08f;
    private float lastPositionY;
    private float lastPositionX;
    private Vector3 lastPosition;
    private Vector3 scalechage;
    private Vector3 targetPosition;
    public Transform target;
    private bool isMoving;
    public bool canTarget;
    bool canMove = true;


    [FMODUnity.EventRef]
    public string InputFootsteps;
    FMOD.Studio.EventInstance FootstepsEvent;
    FMOD.Studio.ParameterInstance WoodParameter;
    FMOD.Studio.ParameterInstance StoneParameter;

    private float WoodValue;
    private float StoneValue;

    public GameObject backgroundTilemap;

    bool canInteract = true;
    public void SetCanInteract(bool value)
    {
        canInteract = value;
    }

    public bool CanPlayerInteract()
    {
        return canInteract;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }
    public bool CanMove()
    {
        return canMove;
    }

    //placing this here bcus fuck
    public void FireWasPutOut(Animator anim)
    {
        if (!GameManagerScript.firePutOut)
        {
            anim.SetTrigger("Splash");
        }
        GameManagerScript.firePutOut = true;

    }

    private void Awake()
    {
        if (GameManagerScript.Instance.hasAwakened == false)
        {
            anim.SetTrigger("WakeUp");
            canTarget = false;
            GameManagerScript.Instance.hasAwakened = true;
        }
    }

    public void Start()
    {
        rbody = gameObject.GetComponent<Rigidbody2D>();
        lastPositionY = transform.position.y;
        lastPositionX = transform.position.x;
        lastPosition = new Vector3(lastPositionX, lastPositionY, transform.position.z);
        targetPosition = transform.position;

        FootstepsEvent = FMODUnity.RuntimeManager.CreateInstance(InputFootsteps);
        FootstepsEvent.getParameter("Wood", out WoodParameter);
        FootstepsEvent.getParameter("Stone", out StoneParameter);

        InvokeRepeating("CallFootsteps", 0, 0.5f);
    }

    void Update()
    {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp_Left") || anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp_Right") || anim.GetCurrentAnimatorStateInfo(0).IsName("WakingUp"))
        {
            canTarget = false;
        }
        else
            canTarget = true;

        WoodParameter.setValue(WoodValue);
        StoneParameter.setValue(StoneValue);

        //Vid Musklick så sätts musens position till target position, om musen inte är över inventoryt.
        if (Input.GetKeyDown(KeyCode.Mouse0) && canTarget)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            bool hitInventory = false;

            var hits = Physics2D.RaycastAll(mousePos, Vector2.zero);
            foreach (var hit in hits)
            {
                if (hit.collider.GetComponent<InventoryUI>() != null)
                {
                    hitInventory = true;
                }
            }

            if (hitInventory == false && canMove)
            {
                target.position = mousePos;
            }

        }

        //kollar om spelaren rör sig
        StartCoroutine("IsMoving");
        target.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        distanceTravelled = Mathf.Abs(transform.position.y - lastPositionY);

        scalechage = new Vector3(transform.localScale.x, transform.localScale.y) * distanceTravelled;

        //Håller koll på om spelaren rör sig i de olika axlarna och sköter animationernas värde
        if ((transform.position.y - lastPositionY) > 0)
        {
            scalechage = -scalechage * offseter;
            transform.Translate(0, 0, distanceTravelled * offseter * Time.deltaTime);
            anim.SetBool("isWalking", true);
            anim.SetFloat("input_y", (transform.position.y - lastPositionY));

        }
        else if ((transform.position.y - lastPositionY) < 0)
        {
            scalechage = scalechage * offseter;
            transform.Translate(0, 0, -distanceTravelled * offseter * Time.deltaTime);
            anim.SetBool("isWalking", true);
            anim.SetFloat("input_y", (transform.position.y - lastPositionY));
        }

        if ((transform.position.x - lastPositionX) > 0)
        {
            anim.SetBool("isWalking", true);
            anim.SetFloat("input_x", (transform.position.x - lastPositionX));
        }
        else if ((transform.position.x - lastPositionX) < 0)
        {
            anim.SetBool("isWalking", true);
            anim.SetFloat("input_x", (transform.position.x - lastPositionX));
        }

        //Startar animationen om spelaren rör sig
        if (isMoving == false)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);

        }

        DetermineTerrain(backgroundTilemap);
        transform.localScale += scalechage;
        lastPositionY = transform.position.y;
        lastPositionX = transform.position.x;
        lastPosition = new Vector3(lastPositionX, lastPositionY, transform.position.z);
    }

    private IEnumerator IsMoving()
    {
        //Kollar ifall spelaren rör sig
        Vector3 startPos = transform.position;
        yield return new WaitForSeconds(0.1f);
        Vector3 finalPos = transform.position;

        if ((finalPos - startPos).sqrMagnitude < 0.005f)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }

    void CallFootsteps()
    {
        if (isMoving == true)
        {
            FootstepsEvent.start();
            //Debug.Log("Souning");
        }
    }

    private void DetermineTerrain(GameObject background)
    {
        float fadetime = 10;
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.back, 100.0f);
        GameObject hit = background;

        if (hit != null)
        {
            if (hit/*.transform.gameObject*/.tag == "wood")
            {
                //Debug.Log("WOOD");
                WoodValue = Mathf.Lerp(WoodValue, 1f, Time.deltaTime * fadetime);
                StoneValue = Mathf.Lerp(StoneValue, 0f, Time.deltaTime * fadetime);
            }
            else if (hit/*.transform.gameObject*/.tag == "stone")
            {
                //Debug.Log("STONE");
                WoodValue = Mathf.Lerp(WoodValue, 0f, Time.deltaTime * fadetime);
                StoneValue = Mathf.Lerp(StoneValue, 1f, Time.deltaTime * fadetime);
            }
            else
            {
                Debug.LogError("Floor is missing tag. Floor needs tags to determine what audio to play for footsteps.", this);
            }
        }
        else
        {
            Debug.LogWarning("Background Object not assigned.", this);
        }
    }
}