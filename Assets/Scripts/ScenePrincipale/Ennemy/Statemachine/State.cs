using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected FiniteStateMachine stateMachine;
    protected Entity entity;

    protected float startTime;
    protected string animBoolName;

    public State(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName) {
        entity = _entity;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }

    public virtual void Enter() {
        startTime = Time.time;
        entity.anim.SetBool(animBoolName, true);
        DoChecks();
    }

    public virtual void Exit() {
        entity.anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate() {
    }

    public virtual void PhysicsUpdate() {
        DoChecks();
    }

    public virtual void DoChecks() {
    }
}
