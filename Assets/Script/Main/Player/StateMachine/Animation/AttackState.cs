using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AttackState : PlayerState
{
    private PlayerCombat playerCombat;

    public AttackState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        this.player = player;

        playerCombat = player.GetComponent<PlayerCombat>();
    }

    public override void EnterState()
    {
        player.Rigidbody2D.linearVelocity = Vector2.zero;
        player.Animator.SetTrigger("Attack");
    }

    public override void FrameUpdate()
    {
        if (!Player.CurrentState.IsName("Attack"))
        {
            player.stateMachine.ChangeState(player.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        if (playerCombat.ActivateAttackPoint())
        {
            playerCombat.TriggerAttack();
        }
    }
}