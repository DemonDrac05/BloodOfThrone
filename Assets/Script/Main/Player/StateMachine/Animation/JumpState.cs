using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpState : PlayerState
{
    private new Player player;

    public JumpState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        this.player = player;
    }

    public override void FrameUpdate()
    {
        if (!player.IsGrounded())
        {
            player.animator.Play("Jump");
        }
        else
        {
            player.stateMachine.ChangeState(player.moveState);
        }
    }
}
