using UnityEngine;

public class BlockState : PlayerState
{
    private PlayerCombat _playerCombat;

    private bool attackBlocked = false;

    private const string BlockSuccessAnimation = "BlockSuccess";
    private const string BlockEndAnimation = "BlockEnd";

    public BlockState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        _playerCombat = player.GetComponent<PlayerCombat>();
    }

    public override void EnterState()
    {
        player.SetVulnerable(false);
    }

    public override void ExitState()
    {
        player.SetVulnerable(true);

        attackBlocked = false;
        _playerCombat.isSucceedBlocking = false;
    }

    public override void FrameUpdate()
    {
        if (EndBlockState())
        {
            player.stateMachine.ChangeState(player.moveState);
        }
        if (!InBlockState())
        {
            player.SetVulnerable(true);
        }
    }

    public override void PhysicsUpdate()
    {
        if (_playerCombat.isSucceedBlocking && !attackBlocked)
        {
            attackBlocked = true;
            player.Animator.SetTrigger(BlockSuccessAnimation);
        }
    }

    bool EndBlockState()
    {
        var animationState = player.Animator.GetCurrentAnimatorStateInfo(0);
        return !animationState.IsName("Block")
            && !animationState.IsName(BlockEndAnimation)
            && !animationState.IsName(BlockSuccessAnimation);
    }

    bool InBlockState()
    {
        var animationState = player.Animator.GetCurrentAnimatorStateInfo(0);
        return animationState.IsName("Block") || animationState.IsName(BlockSuccessAnimation);
    }
}
