using System;
using System.Collections;
using UnityEngine;

public class AttackState : PlayerState
{
    private new Player player;

    public AttackState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        this.player = player;
    }

    public override void FrameUpdate()
    {
        player.animator.Play("Attack");

        var stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Attack") && stateInfo.normalizedTime > 1f)
        {
            player.stateMachine.ChangeState(player.moveState);
        }
    }
}