using System.Collections.Generic;
using UnityEngine;

public enum QuestStepType
{
    COLLECT_ITEM,
    GO_TO_LOCATION,
}
[System.Serializable]
public class IdIntPair
{
    public int Id;
    public int RequiredAmount;
}

[System.Serializable]
public class QuestStepConfig
{
    public QuestStepType stepType;
    public string description;

    //Configuration for different step types
    public List<IdIntPair> itemsToCollect;
    //public int amountToComplete;
    public Vector2 targetPosition; 
    public float triggerRadius;    
}