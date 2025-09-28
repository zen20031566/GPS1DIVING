using UnityEngine;

public enum QuestStepType
{
    COLLECT_TRASH,
    GO_TO_LOCATION,
}

[System.Serializable]
public class QuestStepConfig
{
    public QuestStepType stepType;
    public string description;

    //Configuration for different step types
    public int amountToComplete;
    public Vector2 targetPosition; 
    public float triggerRadius;    
}