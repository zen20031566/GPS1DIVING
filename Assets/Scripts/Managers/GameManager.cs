using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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

        
    }
}
