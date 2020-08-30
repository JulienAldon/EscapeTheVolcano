using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected D_MeleeAttack stateData;
    public MeleeAttackState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, Transform _attackPosition, D_MeleeAttack _stateData) : base(_entity, _stateMachine, _animBoolName, _attackPosition)
    {
        stateData = _stateData;
    }
    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() 
    {
        base.Enter();
        entity.atsm.attackState = this;
        isAnimationFinished = false;
        entity.SetVelocity(0f);
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack() {
        base.TriggerAttack();

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attcakRadius, stateData.whatIsPlayer);
        foreach (Collider2D collider in detectedObjects) {
            collider.transform.SendMessage("Damage", attackPosition.position);
        }
    }

    public override void FinishAttack() {
        base.FinishAttack();
    }
}
