using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager Instance { get; private set; }

    //Events
    public QuestEvents QuestEvents { get; set; }
    public QuestStepEvents QuestStepEvents { get; set; }
    public GameUIEvents GameUIEvents { get; set; }
    public InteractEvent InteractEvent { get; set; }
    public InventoryEvents InventoryEvents { get; set; }
    public ShopEvents ShopEvents { get; set; }
    public GoldEvents GoldEvents { get; set; }
    public EnemyEvents EnemyEvents { get; set; }
    public DialogueEvents DialogueEvents { get; set; }  

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
        QuestEvents = new QuestEvents();
        QuestStepEvents = new QuestStepEvents();
        GameUIEvents = new GameUIEvents();
        InteractEvent = new InteractEvent();
        InventoryEvents = new InventoryEvents();
        ShopEvents = new ShopEvents();
        GoldEvents = new GoldEvents();
        EnemyEvents = new EnemyEvents();
        DialogueEvents = new DialogueEvents();
    }
}
