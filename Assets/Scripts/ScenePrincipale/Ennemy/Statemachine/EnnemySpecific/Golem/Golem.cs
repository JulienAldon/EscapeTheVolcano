﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Entity
{
    public Golem_IdleState idleState { get; private set; }
    public Golem_MoveState moveState { get; private set; }
    public Golem_PlayerDetectedState playerDetectedState { get; private set; }
    public Golem_ChargeState chargeState {get; private set;}
    public Golem_LookForPlayerState lookForPlayerState {get; private set;}
    public Golem_MeleeAttackState meleeAttackState {get; private set;}
    public AudioSource deathSound;
    public AudioSource damageSound;

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
    public GameObject attackAnim;

    private int Health = 5;
    
    public Material matWhite;
    private Material matDefault;
	
    public GameObject gfx;
   
    public override void Start()
    {
        base.Start();
        Team.monsterNumber += 1;

		shake = GameObject.FindGameObjectWithTag ("ScreenShake").GetComponent<Shake> ();
        matDefault = gfx.GetComponent<SpriteRenderer> ().material;

        moveState = new Golem_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new Golem_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new Golem_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
        chargeState = new Golem_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new Golem_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerData, this);
        meleeAttackState = new Golem_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData ,this);
        stateMachine.Initialize(moveState);
    }
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attcakRadius);
    }

    public void Damage(Vector3 dir)
    {
        TakeDamage(0);
    }

    public GameObject splatParticles;
    void OnParticleCollision (GameObject other) {
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (30  * (transform.position.x - other.transform.position.x), 30 * (transform.position.y - other.transform.position.y)));
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.layer == 19) {
			TakeDamage(19);
		}
        else if (collision.gameObject.layer == 12) {
			TakeDamage(12);
        }
	}

	void OnTriggerEnter2D (Collider2D collision) {
		if (collision.gameObject.layer == 19) {
			TakeDamage(19);
		}
	}
    void ResetMaterial () {
        gfx.GetComponent<SpriteRenderer> ().material = matDefault;
    }
     void TakeDamage (int killer) {
        gfx.GetComponent<SpriteRenderer> ().material = matWhite;
        Health -= 1;
        damageSound.Play(0);
        if (Health <= 0)
            StartCoroutine (Death (killer));
        else
            Invoke ("ResetMaterial", .2f);
    }
	IEnumerator Death (int killer) {
		shake.camShake ();
        if (killer == 12) {
            Team.golemKilled += 1;
        }
        deathSound.Play(0);
        // GetComponent<AudioSource>().Play ("MonsterDeath", UnityEngine.Random.Range (1f, 3f));														        
		// SplatCastRay();
		// Time.timeScale = 0.1f;
		yield return new WaitForSeconds (0.4f);
		Instantiate (splatParticles, transform.position, Quaternion.identity);
		// Time.timeScale = 1;
		Destroy (gameObject);
	}
}
