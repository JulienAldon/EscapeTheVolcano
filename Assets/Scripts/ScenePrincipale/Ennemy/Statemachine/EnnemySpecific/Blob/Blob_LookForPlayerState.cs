using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob_LookForPlayerState : LookForPlayerState
{
    private Blob enemy;

    public Blob_LookForPlayerState(Entity _entity, FiniteStateMachine  _stateMachine, string _animBoolName, D_LookForPlayer _stateData, Blob _enemy) : base(_entity, _stateMachine, _animBoolName, _stateData)
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
        if (isPlayerInMinAgroRange) 
        {
            stateMachine.ChangeState(enemy.playerDetectedState);

        } else if (isAllTurnsTimeDone) {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
