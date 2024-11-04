using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyStateMachine : MonoBehaviour
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
}
