using UnityEngine;

public class Ladder : MonoBehaviour, IInteractable
{
    Player player;
    public bool CanInteract { get; set; } = true;

    public void Interact(Player player)
    {
        this.player = player;
        if (player.PlayerStateMachine.CurrentState != player.OnLadderState)
        {
            player.PlayerStateMachine.ChangeState(player.OnLadderState);
            player.transform.position = new Vector3(transform.position.x, transform.position.y - 1, player.transform.position.z);
        }
        else
        {
            player.PlayerStateMachine.ChangeState(player.OnLandState);
        }
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Calculate if player's bottom is above platform's top
            float playerBottom = other.bounds.min.y;
            float playerMiddle = other.bounds.center.y;
            float platformTop = GetComponent<Collider2D>().bounds.max.y;
            float platformBottom = GetComponent<Collider2D>().bounds.min.y;

            if (playerBottom >= platformTop - 0.1f)
            {
                player.PlayerStateMachine.ChangeState(player.OnLandState);
            }
            else if (playerMiddle <= platformBottom - 0.5)
            {
                player.PlayerStateMachine.ChangeState(player.InWaterState);
            }
        }
    }
}
