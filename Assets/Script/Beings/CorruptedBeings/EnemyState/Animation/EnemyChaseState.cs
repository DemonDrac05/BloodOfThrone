using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Henchman Henchman;
    private Player Player;
    public EnemyChaseState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
        if (enemy is Henchman) Henchman = (Henchman)enemy;
    }

    public override void EnterState()
    {

        Henchman.Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        enemy.SetVulnerable(false);
    }

    public override void ExitState()
    {
        enemy.SetVulnerable(true);
        Henchman.Rigidbody2D.bodyType = RigidbodyType2D.Static;
    }

    public override void FrameUpdate()
    {
        if (Henchman != null)
        {
            AnimatorStateInfo animatorState = Henchman.Animator.GetCurrentAnimatorStateInfo(0);
            if (Henchman.ReadyToUltimate() && !animatorState.IsName("Bounce"))
            {
                Henchman.Animator.SetTrigger("Ultimate");
                Debug.Log("Into Bounce State");
                Henchman.stateMachine.ChangeState(Henchman.HenchmanUltimateState);
            }
            else if (Henchman.playerInAttackRange && !animatorState.IsName("Attack"))
            {
                Henchman.Animator.SetTrigger("Reverse");
                Henchman.Animator.SetTrigger("Attack");
                Henchman.stateMachine.ChangeState(Henchman.EnemyAttackState);
            }
            else
            {
                Henchman.Animator.SetTrigger("Chase");
            }
        }
    }

    public override void PhysicsUpdate()
    {
        if (Henchman != null && Henchman.Animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            Henchman.UpdatePosition();
        }
    }
}