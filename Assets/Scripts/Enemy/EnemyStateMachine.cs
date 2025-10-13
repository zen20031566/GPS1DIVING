using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState CurrentState { get; private set; }

    public void Initialize(EnemyState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}

public abstract class EnemyState
{
    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;

    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}
