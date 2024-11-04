using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState playerState;

    public void Initialize(PlayerState firstState)
    {
        playerState = firstState;
        playerState.EnterState();
    }

    public void ChangeState(PlayerState nextState)
    {
        playerState.ExitState();
        playerState = nextState;
        playerState.EnterState();
    }
}
