using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_MeleeAttackState : MeleeAttackState
{
    private Golem enemy;
    public Golem_MeleeAttackState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, Transform _attackPosition, D_MeleeAttack _stateData, Golem _enemy) : base(_entity, _stateMachine, _animBoolName, _attackPosition, _stateData)
    {
        enemy = _enemy;
    }

    public override void DoChecks() {
        base.DoChecks();
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

        if (isAnimationFinished) {
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

    public override void TriggerAttack() {
        base.TriggerAttack();
        enemy.rb.AddForce(new Vector2(-enemy.facingDirection * 5f, 5f), ForceMode2D.Impulse);
        enemy.attackAnim.SetActive(true);
        var a = enemy.attackAnim.GetComponent<ParticleSystem>().main;
        if (enemy.facingDirection > 0)
            a.startRotation = 0;
        else 
            a.startRotation = 180;
    }

    public override void FinishAttack() {
        base.FinishAttack();
        enemy.attackAnim.SetActive(false);
    }
}
