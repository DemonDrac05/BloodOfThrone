using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    protected new Enemy enemy;
    protected Player player;

    private float transformTime;
    private float transformDuration = 0.5f;

    public EnemyChaseState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
        this.enemy = enemy;
        this.player = FindObjectOfType<Player>();
    }

    public override void EnterState()
    {
        transformTime = transformDuration;
        enemy.rb2d.gravityScale = 0f;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        if (transformTime > 0f) 
        {
            transformTime -= Time.deltaTime;
            enemy.animator.Play("transform");
        }
        else if(transformTime <= 0f)
        {
            transformTime = 0f;
            SetMovement();
        }
    }

    public void SetMovement()
    {
        UpdatePosition();
        ChangeState();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    void UpdatePosition()
    {
        enemy.animator.Play("run");

        Vector2 moveDirection = (player.transform.position - enemy.transform.position).normalized;
        moveDirection.y = 0f;
        enemy.Move(moveDirection * enemy.moveSpeed);
    }

    void ChangeState()
    {
        #region Chase -> Idle
        if (enemy.InRangeChaseable == false)
        {
            enemy.rb2d.linearVelocity = Vector2.zero;
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
        #endregion

        #region Chase -> Attack
        if (enemy.InRangeAttackable == true && enemy.attackCDTime == 0f)
        {
            enemy.rb2d.linearVelocity = Vector2.zero;
            enemy.stateMachine.ChangeState(enemy.attackState);
        }
        #endregion
    }
}
