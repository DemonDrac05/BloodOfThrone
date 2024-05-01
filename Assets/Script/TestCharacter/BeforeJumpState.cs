using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeJumpState : PlayerState
{
    public float delayTime;
    public float delayDuration = 0.1f;
    public new PlayerMovement player;
    public BeforeJumpState(PlayerMovement player, PlayerStateMachine stateMachine) : base(player, stateMachine)
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
        player.animator.Play("bJump");

        if(delayTime < 0f)
        {
            delayTime = 0f;
            player.rb2d.velocity = new Vector2(player.rb2d.velocity.x, player.jumpForce);
        }

        if(!player.isGrounded()) player.stateMachine.ChangeState(player.jumpState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
