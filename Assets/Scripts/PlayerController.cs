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
    private float lastPosition;
    public Vector3 scalechage;








    public void Start()
    {
        firstSpeed = speed;
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        diagonalSpeed = (speed / 3) * 2;
        camera = FindObjectOfType<Camera>();
        lastPosition = transform.position.y;
       
    }

    void FixedUpdate()
    {
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

        distanceTravelled = Mathf.Abs(transform.position.y - lastPosition);

        scalechage = new Vector3(transform.localScale.x, transform.localScale.y) * distanceTravelled;


        if ((transform.position.y - lastPosition) > 0)
        {
            scalechage = -scalechage * offseter;
            transform.Translate(0, 0, distanceTravelled * offseter * Time.deltaTime);

        }
        else if ((transform.position.y - lastPosition) < 0)
        {
            scalechage = scalechage * offseter;
            transform.Translate(0, 0, -distanceTravelled * offseter * Time.deltaTime);

        }


        transform.localScale += scalechage;
       
        lastPosition = transform.position.y;
    }



 
    
    
}