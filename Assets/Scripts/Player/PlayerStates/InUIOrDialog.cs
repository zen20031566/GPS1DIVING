using UnityEngine;

public class InUIOrDialog : PlayerState
{
    public InUIOrDialog(PlayerStateMachine playerStateMachine, Player player) : base(playerStateMachine, player) { }

    public override void EnterState()
    {
        player.PlayerController.Rb.linearVelocity = Vector3.zero;
    }

    public override void ExitState()
    {

    }

    public override void FrameUpdate()
    {

    }

    public override void PhysicsUpdate()
    {
        if (player.transform.rotation != Quaternion.identity)
        {
            player.PlayerController.ResetOrientation();
        }
    }
}
