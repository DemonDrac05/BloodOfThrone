using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : PlayerState
{
    public new PlayerMovement player;
    public MovementState(PlayerMovement player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        this.player = player;
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
        player.inputMovement(out player.posX, out player.posY);

        player.rb2d.velocity = new Vector2(player.posX * player.moveSpeed, player.rb2d.velocity.y);

        if (player.isGrounded())
        {
            if (player.posX != 0) player.animator.Play("run");
            else if (player.posX == 0) player.animator.Play("idle");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.stateMachine.ChangeState(player.beforeJumpState);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            player.stateMachine.ChangeState(player.attackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
