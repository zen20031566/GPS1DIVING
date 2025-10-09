using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Player Player;
    public Transform PlayerTransform;

    private void Awake()
    {
        //Singleton
        if (Instance != null)
        {
            Debug.LogError("More than 1 instance of GameManager found");
            Destroy(gameObject);
        }
        Instance = this;

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        PlayerTransform = Player.transform;

        if (Player == null)
            Debug.LogError("Player not found Make sure player has 'Player' tag");
    }
}
