using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerState
{
    public JumpState(PlayerMovement player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        #region Jump Animation
        if (player.rb2d.velocity.y > .1f && !player.isGrounded())
        {
            player.animator.Play("jump");
        }
        #endregion

        #region Fall Animation
        else if (player.rb2d.velocity.y < .1f)
        {
            player.animator.Play("fall");
        }
        #endregion


        #region Jump -> Movement
        if (player.isGrounded())
        {
            player.stateMachine.ChangeState(player.afterJumpState);
        }
        #endregion
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
