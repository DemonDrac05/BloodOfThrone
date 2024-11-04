using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    protected new Enemy enemy;

    private static float transformTime = 0;
    private static float transformDuration = 1.5f;
    public EnemyIdleState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
        this.enemy = enemy;
    }

    public override void EnterState()
    {
        enemy.rb2d.linearVelocity = Vector2.zero;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        SetIdle();
        ChangeState();
    }

    public void SetIdle()
    {
        if (enemy.IsGrounded())
        {
            enemy.rb2d.bodyType = RigidbodyType2D.Static;
            enemy.rb2d.bodyType = RigidbodyType2D.Kinematic;
        }
        if (transformTime < transformDuration)
        {
            transformTime += Time.deltaTime;
            enemy.animator.Play("transform_reverse");
        }
        else if (transformTime >= transformDuration)
        {
            transformTime = transformDuration;
            enemy.animator.Play("idle");
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    void ChangeState()
    {
        #region Idle -> Attack
        if (enemy.InRangeAttackable == true && enemy.attackCDTime == 0f)
        {
            enemy.rb2d.linearVelocity = Vector2.zero;
            enemy.stateMachine.ChangeState(enemy.attackState);
        }
        #endregion

        #region Idle -> Chase
        if (enemy.InRangeAttackable == false && enemy.InRangeChaseable == true)
        {
            enemy.rb2d.linearVelocity = Vector2.zero;
            enemy.stateMachine.ChangeState(enemy.chaseState);
        }
        #endregion
    }
}
