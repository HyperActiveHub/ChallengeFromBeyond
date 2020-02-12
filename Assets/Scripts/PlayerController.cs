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
    public float offseter = 0.00005f;
    private float lastPositionY;
    private float lastPositionX;
    private Vector3 lastPosition;
    public Vector3 scalechage;
    public Vector3 targetPosition;
    public float range = 0.5f;
    public GameObject rayObject;
    private RaycastHit2D hit;






    public void Start()
    {
        firstSpeed = speed;
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        diagonalSpeed = (speed / 3) * 2;
        camera = FindObjectOfType<Camera>();
        lastPositionY = transform.position.y;
        lastPositionX = transform.position.x;
        lastPosition = new Vector3(lastPositionX, lastPositionY, transform.position.z);
        targetPosition = transform.position;
    }


    public void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }


    }


    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, targetPosition.y, transform.position.z), Time.deltaTime * speed);
        

        



      
        
        Vector2 movement_vector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (movement_vector != Vector2.zero)
        {
            anim.SetBool("isWalking", true);
            anim.SetFloat("input_x", movement_vector.x);
            anim.SetFloat("input_y", movement_vector.y);



            if (movement_vector.x != 0 && movement_vector.y != 0)
            {
                speed = diagonalSpeed;
            }
            else
            {
                speed = firstSpeed;
            }


        }
        else
        {
            anim.SetBool("isWalking", false);
        }





        rbody.MovePosition(rbody.position + movement_vector * speed * Time.deltaTime);

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
        

       


        transform.localScale += scalechage;
       
        lastPositionY = transform.position.y;
        lastPositionX = transform.position.x;
        lastPosition = new Vector3(lastPositionX, lastPositionY, transform.position.z);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "wall" && transform.position - lastPosition != Vector3.zero)
        {
            targetPosition = transform.position;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "wall" && transform.position - lastPosition != Vector3.zero)
        {
            targetPosition = transform.position;
        }
    }





}