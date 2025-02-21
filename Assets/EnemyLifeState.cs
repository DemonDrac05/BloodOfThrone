public class EnemyLifeState : EnemyState
{
    private EnemyHealth _enemyHealth;

    public EnemyLifeState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
        _enemyHealth = enemy.GetComponent<EnemyHealth>();
    }


    public override void EnterState()
    {
        enemy.SetVulnerable(false);
        TriggerDamageState();
    }

    public override void ExitState()
    {
        enemy.SetVulnerable(true);
    }

    public override void FrameUpdate()
    {
        if (!_enemyHealth.IsDead())
        {
            if (!Enemy.CurrentState.IsName("Hurt"))
            {
                enemy.stateMachine.ChangeState(enemy.EnemyIdleState);
            }
        }
    }

    private void TriggerDamageState()
    {
        if (_enemyHealth.IsDead())
        {
            enemy.Animator.SetTrigger("Dead");
        }
        else
        {
            enemy.Animator.SetTrigger("Hurt");
        }
        _enemyHealth.UpdateHealthFill();
    }
}
