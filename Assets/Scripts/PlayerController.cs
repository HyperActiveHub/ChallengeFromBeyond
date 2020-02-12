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
    private Vector2 startPos;
    private Vector2 finalPos;





    public void Start()
    {
        firstSpeed = speed;
        
        diagonalSpeed = (speed / 3) * 2;
        camera = FindObjectOfType<Camera>();
        lastPositionY = transform.position.y;
        lastPositionX = transform.position.x;
        lastPosition = new Vector3(lastPositionX, lastPositionY, transform.position.z);
        targetPosition = transform.position;
    }


    public void Update()
    {
        
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //}


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            target.position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
           
        }

        IsMoving();
    }


    void FixedUpdate()
    {
        // transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, targetPosition.y, transform.position.z), Time.deltaTime * speed);






       




        target.position = new Vector3(target.position.x, target.position.y, transform.position.z);


       

        distanceTravelled = Mathf.Abs(transform.position.y - lastPositionY);
        


        scalechage = new Vector3(transform.localScale.x, transform.localScale.y) * distanceTravelled;



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
        //if (rbody.IsSleeping())
        //{
        //    anim.SetBool("isWalking", false);
        //}

        if (isMoving == false)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }





        



        transform.localScale += scalechage;
       
        lastPositionY = transform.position.y;
        lastPositionX = transform.position.x;
        lastPosition = new Vector3(lastPositionX, lastPositionY, transform.position.z);
        
    }

    private IEnumerator IsMoving()
    {

        startPos = transform.position;
        yield return new WaitForSeconds(1f);
        finalPos = transform.position;

        if ((finalPos - startPos).sqrMagnitude > 0.1)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
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





