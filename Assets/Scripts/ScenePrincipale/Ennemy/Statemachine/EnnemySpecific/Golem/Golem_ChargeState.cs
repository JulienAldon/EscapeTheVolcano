using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_ChargeState : ChargeState
{
    private Golem enemy;

    public Golem_ChargeState(Entity _entity, FiniteStateMachine  _stateMachine, string _animBoolName, D_ChargeState _stateData, Golem _enemy) : base(_entity, _stateMachine, _animBoolName, _stateData)
    {
        enemy = _enemy;
    }

    public override void Enter() {
        base.Enter();
        enemy.rb.AddForce(new Vector2(enemy.facingDirection * 5f, 2f), ForceMode2D.Impulse);
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        //enemy.SetVelocity(stateData.chargeSpeed); 
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
