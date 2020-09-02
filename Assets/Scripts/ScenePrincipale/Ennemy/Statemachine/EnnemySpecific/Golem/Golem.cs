using System.Collections;
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
	private AudioManager audioManager;
    public GameObject gfx;
   
    public override void Start()
    {
        base.Start();

        audioManager = FindObjectOfType<AudioManager> ();
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
        TakeDamage();
    }

    public GameObject splatParticles;
    void OnParticleCollision (GameObject other) {
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (30  * (transform.position.x - other.transform.position.x), 30 * (transform.position.y - other.transform.position.y)));
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.layer == 19 || collision.gameObject.layer == 12) {
			TakeDamage();
		}
	}

	void OnTriggerEnter2D (Collider2D collision) {
		if (collision.gameObject.layer == 19) {
			TakeDamage();
		}
	}
    void ResetMaterial () {
        gfx.GetComponent<SpriteRenderer> ().material = matDefault;
    }
     void TakeDamage () {
        gfx.GetComponent<SpriteRenderer> ().material = matWhite;
        Health -= 1;
        if (Health <= 0)
            StartCoroutine (Death ());
        else
            Invoke ("ResetMaterial", .2f);
    }
	IEnumerator Death () {
		shake.camShake ();
        audioManager.Play ("MonsterDeath", UnityEngine.Random.Range (1f, 3f));
        // GetComponent<AudioSource>().Play ("MonsterDeath", UnityEngine.Random.Range (1f, 3f));														        
		// SplatCastRay();
		Instantiate (splatParticles, transform.position, Quaternion.identity);
		// Time.timeScale = 0.1f;
		yield return new WaitForSeconds (0.1f);
		// Time.timeScale = 1;
		Destroy (gameObject);
	}
}
