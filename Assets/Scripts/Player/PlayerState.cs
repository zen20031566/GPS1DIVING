using UnityEngine;

public abstract class PlayerState 
{
    //Player components
    protected PlayerStateMachine playerStateMachine;
    protected Player player;

    public PlayerState(PlayerStateMachine playerStateMachine ,Player player)
    {
        this.playerStateMachine = playerStateMachine;
        this.player = player;
    }

    public virtual void EnterState()
    {

    }

    public virtual void ExitState()
    {

    }

    public virtual void FrameUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }
}
