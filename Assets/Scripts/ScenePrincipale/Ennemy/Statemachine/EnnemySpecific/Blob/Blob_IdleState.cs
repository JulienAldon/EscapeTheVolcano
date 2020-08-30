using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob_IdleState : IdleState
{
    private Blob enemy;

    public Blob_IdleState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_IdleState _stateData, Blob _enemy) : base(_entity, _stateMachine, _animBoolName, _stateData)
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
        } else if (isIdleTimeOver) {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
