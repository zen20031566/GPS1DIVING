using UnityEngine;

public class OnLandState : PlayerState
{
    public OnLandState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player) { }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {

    }

    public override void FrameUpdate()
    {
        //if (player.transform.position.y < player.PlayerController.WaterLevel)
        //{
        //    playerStateMachine.ChangeState(player.OnLandState);
        //}
    }

    public override void PhysicsUpdate()
    {
        player.PlayerController.Move(InputManager.MoveDirection);
        player.PlayerController.GroundCollision();
    }
}
