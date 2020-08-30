using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob_MeleeAttackState : MeleeAttackState
{
    private Blob enemy;
    public Blob_MeleeAttackState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, Transform _attackPosition, D_MeleeAttack _stateData, Blob _enemy) : base(_entity, _stateMachine, _animBoolName, _attackPosition, _stateData)
    {
        enemy = _enemy;
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() 
    {
        base.Enter();
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (isAnimationFinished) {
            if (isPlayerInMinAgroRange) {
                stateMachine.ChangeState(enemy.playerDetectedState);
            } else {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack() {
        base.TriggerAttack();
    }

    public override void FinishAttack() {
        base.FinishAttack();
    }
}
