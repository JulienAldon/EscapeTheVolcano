using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolScript : MonoBehaviour
{
    public float speed;
    public float distance = 2f;
    public float tagRadius = 5f;
    private bool movingRight = true;
    public Transform GroundDetection;
	public Animator animator;
    public float AttackRate;
    private float nextAttack = 0f;
    private bool isAttacking = false;
    private float resetAttackTimer = 0;
    private float resetAttackTime = 2;

    void Update() {
        int layerMask = (LayerMask.GetMask("Ground"));
        RaycastHit2D groundInfo = Physics2D.Raycast(GroundDetection.position, Vector2.down, distance, layerMask);
        if (groundInfo.collider == false) {
            if (movingRight == true) {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            } else {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
        layerMask = (LayerMask.GetMask("Player"));
        move();
        // Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, tagRadius, layerMask); 
        // if (hitCollider == true && Time.time > nextAttack) {
        //     nextAttack += Time.time + AttackRate;
        //     Attack();
        //     print("not moving");
        // } else if (isAttacking == false){
        //     move();
        // }
    }

    void move()
    {
        // animator.ResetTrigger("Attack");
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    
    

    void Attack()
    {
        isAttacking = true;
        // animator.SetTrigger("Attack");
    }

    void EndAttack()
    {
        isAttacking = false;
    }
}
