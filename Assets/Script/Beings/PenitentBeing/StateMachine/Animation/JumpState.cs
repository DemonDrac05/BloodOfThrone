using UnityEngine;

public class JumpState : PlayerState
{
    // --- MOVEMENT PROPERTIES ----------
    private float jumpForce;

    // --- SINGLETON CLASS ----------
    private PlayerMovement playerMovement;
    private PlayerPhysics playerPhysics;

    private AnimatorStateInfo currentAnimatorState;

    public JumpState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        playerPhysics = player.GetComponent<PlayerPhysics>();

        jumpForce = playerMovement.jumpForce;
    }

    public override void EnterState()
    {
        player.Animator.Play(playerMovement.posX == 0 ? "ReadyToJump" : "Jump");
    }

    public override void FrameUpdate()
    {
        currentAnimatorState = player.Animator.GetCurrentAnimatorStateInfo(0);

        MovePlayer();
        if (playerPhysics.isGrounded) ChangeState();
    }

    private void MovePlayer()
    {
        if (playerPhysics.isGrounded)
        {
            if (currentAnimatorState.IsName("Fall"))
            {
                player.Animator.SetTrigger("Land");
            }
            if (currentAnimatorState.IsName("IdleCombat"))
            {
                player.stateMachine.ChangeState(player.moveState);
            }
        }
        else
        {
            string animation = player.Rigidbody2D.linearVelocity.y > .1f ? "Jump" : "Fall";
            if (animation == "Fall" && currentAnimatorState.IsName("Jump"))
            {
                player.Animator.SetTrigger("Fall");
            }
        }
    }

    private void ChangeState()
    {
        if (currentAnimatorState.IsName("Jump"))
        {
            player.Rigidbody2D.linearVelocity = new Vector2(player.Rigidbody2D.linearVelocity.x, jumpForce);
        }
    }
}