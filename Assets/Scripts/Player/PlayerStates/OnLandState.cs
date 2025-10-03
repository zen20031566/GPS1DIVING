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
    }

    public override void PhysicsUpdate()
    {
        if (player.transform.position.y <= player.PlayerController.WaterLevel)
        {
            playerStateMachine.ChangeState(player.InWaterState);
        }

        if (player.transform.rotation != Quaternion.identity)
        {
            player.PlayerController.ResetOrientation();
        }

        player.PlayerController.Rb.gravityScale = player.PlayerController.DefaultGravity;
        player.PlayerController.Move(InputManager.MoveDirection);
        player.PlayerController.Turn(InputManager.MoveDirection);
        player.PlayerController.GroundCollision();
    }
}
