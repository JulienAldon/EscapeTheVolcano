using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob_MoveState : MoveState
{
    private Blob enemy;

    public Blob_MoveState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_MoveState _stateData, Blob _enemy) : base(_entity, _stateMachine, _animBoolName, _stateData)
    {

        enemy = _enemy;
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
        if (isPlayerInMinAgroRange) {
            stateMachine.ChangeState(enemy.playerDetectedState);
        } else if (isDetectingWall || !isDetectingLedge) {
            enemy.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

}
