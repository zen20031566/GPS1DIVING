using System.Collections.Generic;
using UnityEngine;

public enum QuestStepType
{
    COLLECT_ITEM,
    GO_TO_LOCATION,
}
[System.Serializable]
public class RequiredAmountPair
{
    public int Id;
    public ItemTag ItemTag;
    public int RequiredAmount;
}

[System.Serializable]
public class QuestStepConfig
{
    public QuestStepType stepType;
    public string description;

    //Configuration for different step types
    public bool collectByItemTag = false;
    public List<RequiredAmountPair> itemsToCollect;
    public Vector2 targetPosition; 
    public float triggerRadius;    
}