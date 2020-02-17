﻿using UnityEngine;
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


    public void Start()
    {
        rbody = gameObject.GetComponent<Rigidbody2D>();
        lastPositionY = transform.position.y;
        lastPositionX = transform.position.x;
        lastPosition = new Vector3(lastPositionX, lastPositionY, transform.position.z);
        targetPosition = transform.position;
    }


    void Update()
    {



        //Vid Musklick så sätts musens position till target position
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            target.position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

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



}







