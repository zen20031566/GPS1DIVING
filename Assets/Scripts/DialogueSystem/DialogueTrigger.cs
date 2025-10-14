using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    [SerializeField] private DialogueDataSO dialogueData;

    [Header("Interact Settings")]
    [SerializeField] private string interactPrompt = "Press E to Talk";

    [Header("Visual Feedback")]
    [SerializeField] private GameObject interactIndicator; // Optional: UI element above NPC

    private bool playerInRange = false;

    // IInteractable implementation
    public bool CanInteract { get; set; } = true;

    private void Start()
    {
        if (interactIndicator != null)
        {
            interactIndicator.SetActive(false);
        }
    }

    public void Interact(Player player)
    {
        if (!CanInteract) return;

        if (dialogueData != null)
        {
            // Check dialogue conditions
            if (CanStartDialogue())
            {
                DialogueManager.Instance.StartDialogue(dialogueData);
                HideInteractPrompt();
            }
        }
        else
        {
            Debug.LogWarning($"No dialogue data assigned to {gameObject.name}");
        }
    }

    private bool CanStartDialogue()
    {
        // Check if dialogue requires a quest
        if (dialogueData.DialogueData.RequiresQuest)
        {
            string questId = dialogueData.DialogueData.RequiredQuestId;
            QuestState requiredState = dialogueData.DialogueData.RequiredQuestState;

            if (!string.IsNullOrEmpty(questId))
            {
                Quest quest = GameManager.Instance.GetComponent<QuestManager>()?.GetQuestById(questId);
                if (quest == null || quest.State != requiredState)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            ShowInteractPrompt();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            HideInteractPrompt();
        }
    }

    private void ShowInteractPrompt()
    {
        if (interactIndicator != null)
        {
            interactIndicator.SetActive(true);
        }
    }

    private void HideInteractPrompt()
    {
        if (interactIndicator != null)
        {
            interactIndicator.SetActive(false);
        }
    }
}
