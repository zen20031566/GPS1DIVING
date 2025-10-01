using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Scriptable Objects/QuestInfoSO")]
public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }

    //General 
    public string DisplayName;

    //Quest Requirements
    public QuestInfoSO[] QuestPrerequisites;

    //Quest Steps
    //public GameObject[] questStepPrefabs;
    public QuestStepConfig[] QuestStepConfigs;

    //Rewards
    public int GoldReward;

    private void OnValidate()
    {
        #if UNITY_EDITOR
        Id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif

    }
}
