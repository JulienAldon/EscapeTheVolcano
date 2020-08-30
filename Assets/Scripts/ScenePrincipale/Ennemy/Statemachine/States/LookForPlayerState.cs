using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForPlayerState : State
{
    protected bool flipImmediately;
    protected bool isPlayerInMinAgroRange;
    protected bool isAllTurnsDone;
    protected bool isAllTurnsTimeDone;

    protected float lastTurnTime;
    
    protected int amountTurnsDone;
   
    protected D_LookForPlayer stateData;

    public LookForPlayerState(Entity _entity, FiniteStateMachine _stateMachine, string _animBoolName, D_LookForPlayer _stateData) : base(_entity, _stateMachine, _animBoolName)
    {
        stateData = _stateData;
    }

    public override void DoChecks() {
        base.DoChecks();
        
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter() 
    {
        base.Enter();   
        isAllTurnsDone = false;
        isAllTurnsTimeDone = false;
        lastTurnTime = startTime;
        lastTurnTime = startTime;
        amountTurnsDone = 0;

        entity.SetVelocity(0f);
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        
        if (flipImmediately) {
            entity.Flip();
            lastTurnTime = Time.time;
            amountTurnsDone ++;
            flipImmediately = false;
        } else if (Time.time >= lastTurnTime + stateData.timeBetweenTurns && !isAllTurnsDone) {
            entity.Flip();
            lastTurnTime = Time.time;
            amountTurnsDone ++;            
        }
        if (amountTurnsDone >= stateData.amountOfTurns)
        {
            isAllTurnsDone = true;
        }
        if (Time.time >= lastTurnTime + stateData.timeBetweenTurns && isAllTurnsDone)
        {
            isAllTurnsTimeDone = true;
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();     
    }
    public void SetFlipImmediately(bool flip)
    {
        flipImmediately = flip;
    }
     
}
