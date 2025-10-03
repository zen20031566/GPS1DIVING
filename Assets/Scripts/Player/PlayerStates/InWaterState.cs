using Unity.VisualScripting;
using UnityEngine;

public class InWaterState : PlayerState
{
    public InWaterState(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player) { }

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
        if (player.transform.position.y > player.PlayerController.WaterLevel)
        {
            playerStateMachine.ChangeState(player.OnLandState);
        }

        player.PlayerController.Move(InputManager.MoveDirection);
        player.PlayerController.GroundCollision();
    }
}
