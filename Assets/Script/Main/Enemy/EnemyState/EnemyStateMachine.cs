public class EnemyStateMachine
{
    public EnemyState enemyState;

    public void Initialize(EnemyState firstState)
    {
        enemyState = firstState;
        enemyState.EnterState();
    }

    public void ChangeState(EnemyState nextState)
    {
        enemyState.ExitState();
        enemyState = nextState;
        enemyState.EnterState();
    }

    public bool GetCurrentState(EnemyState currentState)
    {
        if (enemyState == currentState) return true;
        return false;
    }
}
