using UnityEngine;

public class AutoDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueDataSO dialogueData;

    [Header("Trigger Type")]
    [SerializeField] private TriggerType triggerType = TriggerType.OnStart;

    [Header("Movement Trigger Settings")]
    [SerializeField] private float movementThreshold = 1f; // Distance player must move
    private Vector3 initialPlayerPosition;
    private bool hasTriggered = false;

    [Header("Quest Trigger Settings")]
    [SerializeField] private string questId; // For quest-based triggers

    [Header("Delay")]
    [SerializeField] private float delayBeforeShow = 0f; // Delay in seconds

    private void Start()
    {
        if (triggerType == TriggerType.OnStart)
        {
            Invoke(nameof(TriggerDialogue), delayBeforeShow);
        }
        else if (triggerType == TriggerType.OnPlayerMove)
        {
            // Store initial position
            initialPlayerPosition = GameManager.Instance.PlayerTransform.position;
        }
    }

    private void OnEnable()
    {
        // Subscribe to quest events if needed
        if (triggerType == TriggerType.OnQuestComplete)
        {
            GameEventsManager.Instance.QuestEvents.OnFinishQuest += CheckQuestComplete;
        }
        else if (triggerType == TriggerType.OnQuestStart)
        {
            GameEventsManager.Instance.QuestEvents.OnStartQuest += CheckQuestStart;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        if (triggerType == TriggerType.OnQuestComplete)
        {
            GameEventsManager.Instance.QuestEvents.OnFinishQuest -= CheckQuestComplete;
        }
        else if (triggerType == TriggerType.OnQuestStart)
        {
            GameEventsManager.Instance.QuestEvents.OnStartQuest -= CheckQuestStart;
        }
    }

    private void Update()
    {
        if (hasTriggered) return;

        if (triggerType == TriggerType.OnPlayerMove)
        {
            CheckPlayerMovement();
        }
    }

    private void CheckPlayerMovement()
    {
        if (GameManager.Instance.PlayerTransform == null) return;

        float distanceMoved = Vector3.Distance(
            initialPlayerPosition,
            GameManager.Instance.PlayerTransform.position
        );

        if (distanceMoved >= movementThreshold)
        {
            TriggerDialogue();
        }
    }

    private void CheckQuestComplete(string completedQuestId)
    {
        if (hasTriggered) return;

        if (completedQuestId == questId)
        {
            Invoke(nameof(TriggerDialogue), delayBeforeShow);
        }
    }

    private void CheckQuestStart(string startedQuestId)
    {
        if (hasTriggered) return;

        if (startedQuestId == questId)
        {
            Invoke(nameof(TriggerDialogue), delayBeforeShow);
        }
    }

    private void TriggerDialogue()
    {
        if (hasTriggered) return;
        if (dialogueData == null)
        {
            Debug.LogWarning($"No dialogue data assigned to {gameObject.name}");
            return;
        }

        hasTriggered = true;
        DialogueManager.Instance.StartDialogue(dialogueData);

        // Optionally destroy this trigger after use
        // Destroy(this);
    }

    public void ManualTrigger()
    {
        TriggerDialogue();
    }
}

public enum TriggerType
{
    OnStart,           // Triggers when game loads
    OnPlayerMove,      // Triggers when player moves X distance
    OnQuestStart,      // Triggers when specific quest starts
    OnQuestComplete,   // Triggers when specific quest completes
    Manual             // Call ManualTrigger() from code
}
