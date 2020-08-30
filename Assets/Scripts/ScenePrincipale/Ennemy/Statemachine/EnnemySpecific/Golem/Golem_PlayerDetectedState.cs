using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_PlayerDetectedState : PlayerDetectedState
{
    private Golem enemy;

    public Golem_PlayerDetectedState(Entity _entity, FiniteStateMachine  _stateMachine, string _animBoolName, D_PlayerDetected _stateData, Golem _enemy) : base(_entity, _stateMachine, _animBoolName, _stateData)
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
        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.meleeAttackState);
        } else if (performLongRangeAction) {
            stateMachine.ChangeState(enemy.chargeState);
        } else if (!isPlayerInMaxAgroRange) {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
