using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float firstSpeed;
    public float diagonalSpeed;
    public Animator anim;
    public Rigidbody2D rbody = null;
    private new Camera camera;
    public float distanceTravelled = 0;
    public float Moved = 0;
    public float offseter = 0.00005f;
    private float lastPositionY;
    private float lastPositionX;
    private Vector3 lastPosition;
    public Vector3 scalechage;
    public Vector3 targetPosition;
    public float range = 0.5f;
    public GameObject rayObject;
    private RaycastHit2D hit;
    public Transform target;
    public bool isMoving;


    [FMODUnity.EventRef]
    public string InputFootsteps;
    FMOD.Studio.EventInstance FootstepsEvent;
    FMOD.Studio.ParameterInstance WoodParameter;
    FMOD.Studio.ParameterInstance StoneParameter;

    private float WoodValue;
    private float StoneValue;



    public void Start()
    {
        firstSpeed = speed;
        
        diagonalSpeed = (speed / 3) * 2;
        camera = FindObjectOfType<Camera>();
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
        WoodParameter.setValue(WoodValue);
        StoneParameter.setValue(StoneValue);


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            target.position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
           
        }

        
    }


    void FixedUpdate()
    {
        // transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, targetPosition.y, transform.position.z), Time.deltaTime * speed);








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

            DetermineTerrain();


        transform.localScale += scalechage;
       
        lastPositionY = transform.position.y;
        lastPositionX = transform.position.x;
        lastPosition = new Vector3(lastPositionX, lastPositionY, transform.position.z);
        
    }

    private IEnumerator IsMoving()
    {
        
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
            Debug.Log("Souning");
        }
     
    }

    private void DetermineTerrain()
    {

        float fadetime = 10;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 100.0f);



        if (hit.transform.gameObject.tag == "wood")
        {
            Debug.Log("WOOD");
            WoodValue = Mathf.Lerp(WoodValue, 1f, Time.deltaTime * fadetime);
            StoneValue = Mathf.Lerp(StoneValue, 0f, Time.deltaTime * fadetime);
        }
        else if (hit.transform.gameObject.tag == "stone")
        {
            Debug.Log("STONE");
            WoodValue = Mathf.Lerp(WoodValue, 0f, Time.deltaTime * fadetime);
            StoneValue = Mathf.Lerp(StoneValue, 1f, Time.deltaTime * fadetime);
        }

    }






}


    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "wall" && transform.position - lastPosition != Vector3.zero)
    //    {
    //        targetPosition = transform.position;
    //    }
    //}

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "wall" && transform.position - lastPosition != Vector3.zero)
    //    {
    //        targetPosition = transform.position;
    //    }
    //}





