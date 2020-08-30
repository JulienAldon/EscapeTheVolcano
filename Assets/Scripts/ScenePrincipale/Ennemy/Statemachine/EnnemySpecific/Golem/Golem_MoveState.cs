using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_MoveState : MoveState
{
    private Golem enemy;

    public Golem_MoveState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_MoveState _stateData, Golem _enemy) : base(_entity, _stateMachine, _animBoolName, _stateData)
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
        enemy.SetVelocity(stateData.movementSpeed);
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
