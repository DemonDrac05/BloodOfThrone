using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterJumpState : PlayerState
{
    public float delayTime;
    public float delayDuration = 0.1f;
    public new PlayerMovement player;
    public AfterJumpState(PlayerMovement player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        this.player = player;
    }

    public override void EnterState()
    {
        delayTime = delayDuration;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        delayTime -= Time.deltaTime;
        player.animator.Play("aJump");

        if(delayTime <= 0f) { player.stateMachine.ChangeState(player.movementState); }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
