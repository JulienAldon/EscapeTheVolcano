using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Entity
{
    public Spikes_IdleState idleState { get; private set; }
    public Spikes_PlayerDetectedState playerDetectedState { get; private set; }
    public Spikes_MeleeAttackState meleeAttackState {get; private set;}
    public AudioSource detectedSound;
    public AudioSource attackSound;
    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_PlayerDetected playerDetectedData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private Transform meleeAttackPosition;
    public bool isTriggered = false;

    public void DetectedSound()
    {
        detectedSound.Play(0);
    }

    public void AttackSound()
    {
        attackSound.Play(0);
    }

    public override void Start()
    {
        base.Start();
        idleState = new Spikes_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new Spikes_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
        meleeAttackState = new Spikes_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData ,this);
        stateMachine.Initialize(idleState);
    }
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attcakRadius);
    }

	void OnTriggerEnter2D (Collider2D collision) {
        isTriggered = true;
	}
}
