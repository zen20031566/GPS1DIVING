using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager Instance { get; private set; }

    //Events
    public TrashEvents TrashEvents { get; set; }
    public WeightEvents WeightEvents { get; set; }
    public QuestEvents QuestEvents { get; set; }

    private void Awake()
    {
        //Singleton
        if (Instance != null)
        {
            Debug.LogError("More than 1 instance of GameEventManager found");
            Destroy(gameObject);
        }
        Instance = this;

        //Initialize events
        TrashEvents = new TrashEvents();
        WeightEvents = new WeightEvents();
        QuestEvents = new QuestEvents();
    }


}
