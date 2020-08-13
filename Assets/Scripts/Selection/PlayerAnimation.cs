using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private select sel;
    public static int count = 0;
    public Animator animator;
    public float[] placement;
    public float[] worldPlacement;
    private int moveState;
    private float step;
    public float speed = 5f;
    private Vector3 finalPos;
    private Vector3 finalWorldPos;
    // Start is called before the first frame update
    void Start()
    {
        sel = GameObject.Find("selectTable").GetComponent<select>();
        moveState = 0;
        finalPos = new Vector3(placement[count], 1, 0);
        finalWorldPos = new Vector3(worldPlacement[count], 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (moveState == 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;            
            step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, transform.position.y, transform.position.z), step);
        }
        if (transform.position.x == 0 && moveState == 0) {
            sel.setCanPressAgain();
            animator.SetTrigger("endMove");
            moveState = 1;
        }
        if (moveState == 2) {
            step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(12, transform.position.y, transform.position.z), step);
        }
        if (transform.position.x == 12 && moveState == 2)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            moveState = 4;
        }
        if (moveState == 3) {
            step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(-12, transform.position.y, transform.position.z), step); 
        }
        if (transform.position.x == -12 && moveState == 3) {
            GetComponent<SpriteRenderer>().flipX = false;
            moveState = 5;
        }
     
        if (moveState == 4)
        {
            transform.localScale = new Vector3(1, 1, 0);            
            step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(6, transform.position.y, transform.position.z), step);
        }
        if (transform.position.x == 6 && moveState == 4) {
            //GetComponent<SpriteRenderer>().flipX = false;
            moveState = 6;
        }
        if (moveState == 5)
        {
            transform.localScale = new Vector3(1, 1, 0);            
            step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(-6, transform.position.y, transform.position.z), step);
        }
        if (transform.position.x == -6 && moveState == 5) {
            //GetComponent<SpriteRenderer>().flipX = true;            
            moveState = 7;
        }
        if (moveState == 7) {
            moveState = 9;
            refused();
        }
         if (moveState == 6) {
            moveState = 8;
            accepted();
        }
        if (moveState == 8) {
            step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, finalWorldPos, step);
        }
        if (moveState == 8 && transform.position.x == finalWorldPos.x)
        {
            moveState = 10;
            animator.SetTrigger("endSelected");
            transform.position = new Vector3(transform.position.x, transform.position.y, 2);
        }
    }

    void accepted()
    {   
        count += 1;
        if (count <= 3) {
            sel.regenerateCard();
        }
    }

    void refused()
    {
        sel.regenerateCard();

        animator.SetTrigger("endRefused");

        Destroy(gameObject, 1f);
    }

    public void accept()
    {
        GetComponent<SpriteRenderer>().flipX = false;
        animator.SetTrigger("Selected");
        moveState = 2;
    }

    public void refuse()
    {
        animator.SetTrigger("Refused");
        moveState = 3;
    }
}
