public class PlayerStateMachine
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
