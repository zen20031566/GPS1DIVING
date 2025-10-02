using UnityEngine;

public abstract class PlayerState 
{
    //Player components
    protected PlayerStateMachine playerStateMachine;
    protected PlayerController playerController;

    public PlayerState(PlayerStateMachine playerStateMachine ,PlayerController playerController)
    {
        this.playerStateMachine = playerStateMachine;
        this.playerController = playerController;
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
