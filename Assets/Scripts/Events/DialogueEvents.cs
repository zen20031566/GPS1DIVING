using System;

public class DialogueEvents
{
    public event Action<string> OnDialogueStarted;
    public event Action<string> OnDialogueEnded;
    public event Action<string> OnDialogueLineChanged;

    public void DialogueStarted(string dialogueId)
    {
        OnDialogueStarted?.Invoke(dialogueId);
    }

    public void DialogueEnded(string dialogueId)
    {
        OnDialogueEnded?.Invoke(dialogueId);
    }

    public void DialogueLineChanged(string lineText)
    {
        OnDialogueLineChanged?.Invoke(lineText);
    }
}
