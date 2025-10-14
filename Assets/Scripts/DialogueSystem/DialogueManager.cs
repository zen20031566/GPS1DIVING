using System.Collections;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private DialogueUI dialogueUI;

    private DialogueData currentDialogue;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;

    public bool IsDialogueActive => isDialogueActive;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (!isDialogueActive) return;

        // Progress dialogue on interact input
        if (InputManager.InteractPressed)
        {
            if (isTyping)
            {
                // Skip typing animation
                StopAllCoroutines();
                dialogueUI.ShowFullLine(currentDialogue.Lines[currentLineIndex].Text);
                isTyping = false;
            }
            else
            {
                // Move to next line or end dialogue
                NextLine();
            }
        }
    }

    public void StartDialogue(DialogueDataSO dialogueDataSO)
    {
        if (dialogueDataSO == null || dialogueDataSO.DialogueData.Lines.Length == 0)
        {
            Debug.LogWarning("DialogueDataSO is null or has no lines!");
            return;
        }

        currentDialogue = dialogueDataSO.DialogueData;
        currentLineIndex = 0;
        isDialogueActive = true;

        // Notify events
        GameEventsManager.Instance.DialogueEvents.DialogueStarted(currentDialogue.DialogueId);

        // Set player to dialogue state
        Player player = GameManager.Instance.Player;
        if (player != null)
        {
            player.PlayerStateMachine.ChangeState(player.OnUIOrDialog);
        }

        // Show UI and first line
        dialogueUI.ShowDialogueBox();
        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        if (currentLineIndex >= currentDialogue.Lines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentDialogue.Lines[currentLineIndex];

        // Update UI
        dialogueUI.SetSpeakerName(line.SpeakerName);
        dialogueUI.SetPortrait(line.SpeakerPortrait);

        // Start typing animation
        StartCoroutine(TypeLine(line.Text));
    }

    private IEnumerator TypeLine(string text)
    {
        isTyping = true;
        dialogueUI.ClearText();

        foreach (char c in text)
        {
            dialogueUI.AppendCharacter(c);
            yield return new WaitForSeconds(0.03f); // Typing speed
        }

        isTyping = false;
    }

    private void NextLine()
    {
        currentLineIndex++;
        ShowCurrentLine();
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialogueUI.HideDialogueBox();

        // Handle dialogue actions
        HandleDialogueActions();

        // Notify events
        GameEventsManager.Instance.DialogueEvents.DialogueEnded(currentDialogue.DialogueId);

        // Return player to previous state
        Player player = GameManager.Instance.Player;
        if (player != null)
        {
            // Determine which state to return to based on water level
            if (player.PlayerHead.transform.position.y >= player.PlayerController.WaterLevel)
            {
                player.PlayerStateMachine.ChangeState(player.OnLandState);
            }
            else
            {
                player.PlayerStateMachine.ChangeState(player.InWaterState);
            }
        }

        currentDialogue = null;
        currentLineIndex = 0;
    }

    private void HandleDialogueActions()
    {
        if (currentDialogue.StartsQuest && !string.IsNullOrEmpty(currentDialogue.QuestToStartId))
        {
            GameEventsManager.Instance.QuestEvents.StartQuest(currentDialogue.QuestToStartId);
        }

        if (currentDialogue.CompletesQuest && !string.IsNullOrEmpty(currentDialogue.QuestToCompleteId))
        {
            GameEventsManager.Instance.QuestEvents.FinishQuest(currentDialogue.QuestToCompleteId);
        }
    }

    public void ForceEndDialogue()
    {
        if (isDialogueActive)
        {
            StopAllCoroutines();
            EndDialogue();
        }
    }
}
