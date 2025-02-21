using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private EnemyCombat EnemyCombat;
    private Henchman Henchman;

    private bool performAttacked;
    public EnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
        EnemyCombat = enemy.GetComponent<EnemyCombat>();
        if (enemy is Henchman) Henchman = (Henchman)enemy;
    }

    public override void EnterState()
    {
        enemy.SetVulnerable(true);
        performAttacked = false;
    }

    public override void ExitState()
    {
        if (Henchman != null) Henchman.SetActionCooldown();
        performAttacked = false;
    }

    public override void FrameUpdate()
    {
        if (!enemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            enemy.stateMachine.ChangeState(enemy.EnemyIdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        if (EnemyCombat.ActivateAttackPoint() && !performAttacked)
        {
            performAttacked = true;
            EnemyCombat.TriggerAttack(7.5f);
        }
    }
}
