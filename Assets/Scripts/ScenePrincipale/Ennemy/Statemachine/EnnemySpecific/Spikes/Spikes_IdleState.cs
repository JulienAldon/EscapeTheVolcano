using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes_IdleState : IdleState
{
    private Spikes enemy;

    public Spikes_IdleState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_IdleState _stateData, Spikes _enemy) : base(_entity, _stateMachine, _animBoolName, _stateData)
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
        if (enemy.isTriggered) {
            stateMachine.ChangeState(enemy.playerDetectedState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
