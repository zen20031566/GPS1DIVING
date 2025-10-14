using UnityEngine;

public class OnLadderState : PlayerState
{
    public OnLadderState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player) { }

    public override void EnterState()
    {
        player.PlayerController.Rb.gravityScale = 0;
        player.PlayerController.Rb.linearVelocity = Vector2.zero;   
        player.transform.rotation = Quaternion.identity;
    }

    public override void ExitState()
    {

    }

    public override void FrameUpdate()
    {

    }

    public override void PhysicsUpdate()
    {
        player.PlayerController.LadderMove(InputManager.MoveDirection);
    }
}
