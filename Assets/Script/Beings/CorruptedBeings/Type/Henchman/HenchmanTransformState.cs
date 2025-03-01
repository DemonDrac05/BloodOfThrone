using UnityEngine;

public class HenchmanTransformState : EnemyState
{
    private Henchman Henchman;

    public HenchmanTransformState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
        if (enemy is Henchman) Henchman = (Henchman)enemy;
    }

    public override void EnterState()
    {
        Henchman.SetVulnerable(false);
    }

    public override void FrameUpdate()
    {
        AnimatorStateInfo animatorState = Henchman.Animator.GetCurrentAnimatorStateInfo(0);
        if (Henchman.ReadyToUltimate() && !animatorState.IsName("Bounce"))
        {
            Henchman.Animator.SetTrigger("Ultimate");
            Henchman.stateMachine.ChangeState(Henchman.HenchmanUltimateState);
        }
        else if (Henchman.playerInAttackRange && !animatorState.IsName("Attack"))
        {
            Henchman.Animator.SetTrigger("Reverse");
            Henchman.Animator.SetTrigger("Attack");
            Henchman.stateMachine.ChangeState(Henchman.EnemyAttackState);
        }
        else if (!animatorState.IsName("Run"))
        {
            Henchman.Animator.SetTrigger("Chase");
            Henchman.stateMachine.ChangeState(Henchman.EnemyChaseState);
        }
    }
}
