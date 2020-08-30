using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob_ChargeState : ChargeState
{
    private Blob enemy;

    public Blob_ChargeState(Entity _entity, FiniteStateMachine  _stateMachine, string _animBoolName, D_ChargeState _stateData, Blob _enemy) : base(_entity, _stateMachine, _animBoolName, _stateData)
    {
        enemy = _enemy;
    }

    public override void Enter() {
        base.Enter();
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        if (performCloseRangeAction) {
                stateMachine.ChangeState(enemy.meleeAttackState);
        } else if (!isDetectingLedge || isDetectingWall) {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        } else if (isChargeTimeOver) {
            
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

}
