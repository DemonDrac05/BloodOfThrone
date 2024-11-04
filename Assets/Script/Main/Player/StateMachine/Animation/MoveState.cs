using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerState
{
    private new Player player;

    private float moveSpeed;
    private float jumpForce;

    public MoveState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        this.player = player;

        this.moveSpeed = player.moveSpeed;
        this.jumpForce = player.jumpForce;
    }

    public override void EnterState()
    {
        player.AllowToFlip = true;
    }

    public override void ExitState()
    {
        player.AllowToFlip = false;
    }

    public override void FrameUpdate()
    {
        if (player.IsGrounded())
        {
            if (player.posX != 0)
            {
                player.animator.Play("Run");
                player.rb2d.linearVelocityX = player.posX * moveSpeed;
            }
            else 
            {
                player.animator.Play("Idle");
                player.rb2d.linearVelocityX = 0f;
            }
        }
        ChangeState();
    }

    private void ChangeState()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            player.rb2d.linearVelocity = new Vector2(player.rb2d.linearVelocity.x, jumpForce);
            player.stateMachine.ChangeState(player.jumpState);
        }
        if (Input.GetKey(KeyCode.K))
        {
            player.rb2d.linearVelocity = Vector2.zero;
            player.stateMachine.ChangeState(player.attackState);
        }
    }
}
