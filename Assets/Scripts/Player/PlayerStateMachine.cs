using UnityEngine;

public class PlayerStateMachine 
{
    public PlayerState CurrentState { get; set; }

    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.EnterState();
    }

    private void ChangeState(PlayerState newState)
    {
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }
}
