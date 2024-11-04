using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    protected new Enemy enemy;

    private float attackTime;
    private float attackDuration = 0.75f;

    public EnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
        this.enemy = enemy;
    }

    public override void EnterState()
    {
        attackTime = attackDuration;
        enemy.rb2d.bodyType = RigidbodyType2D.Kinematic;
    }

    public override void ExitState()
    {
        enemy.attackCDTime = enemy.attackCDDuration;
        enemy.rb2d.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void FrameUpdate()
    {
        if(attackTime < 0f)
        {
            attackTime = 0f;
            
        }
        if(attackTime == 0f)
        {
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        if (attackTime > 0f)
        {
            attackTime -= Time.deltaTime;
            enemy.animator.Play("attack");
        }

        enemy.rb2d.linearVelocity = Vector2.zero;
    }
}
