using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image speakerPortrait;
    [SerializeField] private GameObject continueIndicator; // Arrow or "Press E"

    [Header("Settings")]
    [SerializeField] private bool hideSpeakerIfEmpty = true;
    [SerializeField] private GameObject speakerPanel; // Optional: panel containing name/portrait

    private void Start()
    {
        HideDialogueBox();
    }

    public void ShowDialogueBox()
    {
        dialogueBox.SetActive(true);
        continueIndicator.SetActive(false);
    }

    public void HideDialogueBox()
    {
        dialogueBox.SetActive(false);
    }

    public void SetSpeakerName(string name)
    {
        if (speakerNameText != null)
        {
            speakerNameText.text = name;

            if (hideSpeakerIfEmpty && speakerPanel != null)
            {
                speakerPanel.SetActive(!string.IsNullOrEmpty(name));
            }
        }
    }

    public void SetPortrait(Sprite portrait)
    {
        if (speakerPortrait != null)
        {
            if (portrait != null)
            {
                speakerPortrait.sprite = portrait;
                speakerPortrait.enabled = true;
            }
            else
            {
                speakerPortrait.enabled = false;
            }
        }
    }

    public void ClearText()
    {
        if (dialogueText != null)
        {
            dialogueText.text = "";
        }
        continueIndicator.SetActive(false);
    }

    public void AppendCharacter(char c)
    {
        if (dialogueText != null)
        {
            dialogueText.text += c;
        }
    }

    public void ShowFullLine(string text)
    {
        if (dialogueText != null)
        {
            dialogueText.text = text;
        }
        continueIndicator.SetActive(true);
    }
}
