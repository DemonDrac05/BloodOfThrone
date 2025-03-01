using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerState
{
    // --- MOVEMENT PROPERTIES ----------
    private float moveSpeed;
    private float jumpForce;

    // --- ANIMATION NAME ----------
    private const string Idle = "Idle";
    private const string Run = "Run";

    // --- ANIMATION NAME ----------
    private const string IsRunning = "isRunning";
    private const string IsJumping = "isJumping";
    private const string TriggerAttack = "Attack";

    // --- SINGLETON CLASS ----------
    private PlayerMovement playerMovement;
    private PlayerPhysics playerPhysics;

    public MoveState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        playerPhysics = player.GetComponent<PlayerPhysics>();

        moveSpeed = playerMovement.moveSpeed;
        jumpForce = playerMovement.jumpForce;
    }

    public override void EnterState()
    {
        player.SetFlip(true);
    }

    public override void ExitState()
    {
        player.SetFlip(false);
    }

    public override void FrameUpdate()
    {
        MovePlayer();
        if (playerPhysics.IsGrounded()) ChangeState();
    }

    private void MovePlayer()
    {
        if (playerPhysics.IsGrounded())
        {
            if (playerMovement.posX != 0)
            {
                player.Animator.SetBool(IsRunning, true);
                player.Rigidbody2D.linearVelocityX = playerMovement.posX * moveSpeed;
            }
            else
            {
                player.Animator.SetBool(IsRunning, false);
                player.Rigidbody2D.linearVelocityX = 0f;
            }
        }
    }

    private void ChangeState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.stateMachine.ChangeState(player.jumpState);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            player.stateMachine.ChangeState(player.attackState);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            player.Animator.SetTrigger("Block");
            player.stateMachine.ChangeState(player.blockState);
        }
    }
}
