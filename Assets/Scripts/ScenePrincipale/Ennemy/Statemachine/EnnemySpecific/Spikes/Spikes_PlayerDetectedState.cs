using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes_PlayerDetectedState : PlayerDetectedState
{
    private Spikes enemy;

    public Spikes_PlayerDetectedState(Entity _entity, FiniteStateMachine  _stateMachine, string _animBoolName, D_PlayerDetected _stateData, Spikes _enemy) : base(_entity, _stateMachine, _animBoolName, _stateData)
    {
        enemy = _enemy;
    }

    public override void Enter() {
        base.Enter();
        enemy.DetectedSound();
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        if (performLongRangeAction)
        {
            stateMachine.ChangeState(enemy.meleeAttackState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
