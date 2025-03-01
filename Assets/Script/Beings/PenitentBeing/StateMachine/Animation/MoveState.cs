using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveState : PlayerState
{
    // --- SINGLETON CLASS ----------
    private PlayerMovement playerMovement;
    private PlayerPhysics playerPhysics;

    public MoveState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        playerPhysics  = player.GetComponent<PlayerPhysics>();
    }

    public override void EnterState()
    {
        player.SetFlip(true);

        player.Inputs.Player.Jump.started += OnJump;
    }

    public override void ExitState()
    {
        player.SetFlip(false);

        player.Inputs.Player.Jump.started -= OnJump;
    }

    public override void FrameUpdate()
    {
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.Space) && playerPhysics.isGrounded)
        {

            player.stateMachine.ChangeState(player.jumpState);
        } 
    }

    private void MovePlayer()
    {
        player.Rigidbody2D.linearVelocityX = playerMovement.posX * playerMovement.moveSpeed;
        player.Animator.SetBool("isRunning", playerMovement.posX != 0 && playerPhysics.isGrounded);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!playerPhysics.isGrounded) return;
    }
}