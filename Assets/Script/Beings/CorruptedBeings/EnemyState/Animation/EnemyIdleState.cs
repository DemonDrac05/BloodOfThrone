using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private Henchman Henchman;

    public EnemyIdleState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
        if (enemy is Henchman) Henchman = (Henchman)enemy;
    }

    public override void EnterState()
    {
        enemy.SetVulnerable(true);
    }

    public override void ExitState()
    {

    }

    public override void FrameUpdate()
    {
        if (Henchman != null)
        {
            AnimatorStateInfo animatorState = Henchman.Animator.GetCurrentAnimatorStateInfo(0);
            if (Henchman.ReadyToAction())
            {
                if (Henchman.ReadyToUltimate() && !animatorState.IsName("Bounce"))
                {
                    Henchman.Animator.SetTrigger("Ultimate");
                    Henchman.stateMachine.ChangeState(Henchman.HenchmanUltimateState);
                }
                else if (Henchman.playerInAttackRange && !animatorState.IsName("Attack"))
                {
                    Henchman.Animator.SetTrigger("Attack");
                    Henchman.stateMachine.ChangeState(Henchman.EnemyAttackState);
                }
                else if (!animatorState.IsName("TransformToSmokeMode"))
                {
                    Henchman.Animator.SetTrigger("Transform");
                    Henchman.stateMachine.ChangeState(Henchman.HenchmanTransformState);
                }
            }
            else
            {
                AnimatorExtensions.ResetAllTriggers(Henchman.Animator);
            }
        }
    }
}
