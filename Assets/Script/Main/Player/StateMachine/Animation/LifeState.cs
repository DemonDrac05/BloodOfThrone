using UnityEngine;

public class LifeState : PlayerState
{
    private PlayerHealth _playerHealth;

    public LifeState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        _playerHealth = player.GetComponent<PlayerHealth>();
    }

    public override void EnterState()
    {
        player.SetVulnerable(false);
        TriggerDamageState();
    }

    public override void ExitState()
    {
        player.SetVulnerable(true);
    }

    public override void FrameUpdate()
    {
        if (!_playerHealth.IsDead())
        {
            if (!Player.CurrentState.IsName("Hurt"))
            {
                player.stateMachine.ChangeState(player.moveState);
            }
        }
    }

    private void TriggerDamageState()
    {
        if (_playerHealth.IsDead())
        {
            player.Animator.SetTrigger("Dead");
        }
        else
        {
            player.Animator.SetTrigger("Hurt");
        }
        _playerHealth.UpdateHealthFill();
    }
}
