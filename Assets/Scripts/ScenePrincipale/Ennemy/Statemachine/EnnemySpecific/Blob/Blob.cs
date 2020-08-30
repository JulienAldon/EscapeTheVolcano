﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : Entity
{
    public Blob_IdleState idleState { get; private set; }
    public Blob_MoveState moveState { get; private set; }
    public Blob_PlayerDetectedState playerDetectedState { get; private set; }
    public Blob_ChargeState chargeState {get; private set;}
    public Blob_LookForPlayerState lookForPlayerState {get; private set;}
    public Blob_MeleeAttackState meleeAttackState {get; private set;}

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_PlayerDetected playerDetectedData;
    [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_LookForPlayer lookForPlayerData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private Transform meleeAttackPosition;

	private Shake shake;
	public bool canRespawn = true;


    public override void Start()
    {
        base.Start();

        // audio = FindObjectOfType<AudioManager> ();		        		
		shake = GameObject.FindGameObjectWithTag ("ScreenShake").GetComponent<Shake> ();

        moveState = new Blob_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new Blob_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new Blob_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
        chargeState = new Blob_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new Blob_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerData, this);
        meleeAttackState = new Blob_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData ,this);
        stateMachine.Initialize(moveState);
    }
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attcakRadius);
    }

    public GameObject splatParticles;
    void OnParticleCollision (GameObject other) {
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (30  * (transform.position.x - other.transform.position.x), 30 * (transform.position.y - other.transform.position.y)));
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.layer == 19 || collision.gameObject.layer == 12) {
			StartCoroutine (Death ());
		}
	}

	void OnTriggerEnter2D (Collider2D collision) {
		if (collision.gameObject.layer == 19) {
			StartCoroutine (Death ());
		}
	}
	IEnumerator Death () {
		shake.camShake ();
        // GetComponent<AudioSource>().Play ("MonsterDeath", UnityEngine.Random.Range (1f, 3f));														        
		// SplatCastRay();
		Instantiate (splatParticles, transform.position, Quaternion.identity);
		// Time.timeScale = 0.1f;
		yield return new WaitForSeconds (0.1f);
		// Time.timeScale = 1;
		if (canRespawn) {
			var Child1 = Instantiate (this.gameObject, transform.position, Quaternion.identity);
			Child1.GetComponent<Blob> ().canRespawn = false;
			Child1.transform.localScale = new Vector3 (.5f, .5f, 1);
            Child1.GetComponent<Rigidbody2D> ().AddForce(new Vector2(-10, 10));
			var Child2 = Instantiate (this.gameObject, transform.position, Quaternion.identity);
			Child2.transform.localScale = new Vector3 (.5f, .5f, 1);
			Child2.GetComponent<Blob> ().canRespawn = false;
            Child2.GetComponent<Rigidbody2D> ().AddForce(new Vector2(10, 10));
		}
		Destroy (gameObject);
	}
}