using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes_MeleeAttackState : MeleeAttackState
{
    private Spikes enemy;
    public Spikes_MeleeAttackState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, Transform _attackPosition, D_MeleeAttack _stateData, Spikes _enemy) : base(_entity, _stateMachine, _animBoolName, _attackPosition, _stateData)
    {
        enemy = _enemy;
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() 
    {
        base.Enter();
        enemy.AttackSound();
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
    }

    public override void FinishAttack() {
        base.FinishAttack();
        stateMachine.ChangeState(enemy.idleState);
        enemy.isTriggered = false;
    }
}
