using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Scriptable Objects/QuestInfoSO")]
public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    //General 
    public string displayName;

    //Quest Requirements
    public QuestInfoSO[] questPrerequisites;

    //Quest Steps
    public GameObject[] questStepPrefabs;

    //Rewards
    public int goldReward;

    private void OnValidate()
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif

    }
}
