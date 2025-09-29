using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    [SerializeField] private GameObject canStartIcon;
    [SerializeField] private GameObject canFinishIcon;

    public void SetState(QuestState newState)
    {
        canStartIcon.SetActive(false);
        canFinishIcon.SetActive(false);

        if (newState.Equals(QuestState.CAN_START))
        {
            canStartIcon.SetActive(true);
        }
        else if (newState.Equals(QuestState.CAN_FINISH))
        {
            canFinishIcon.SetActive(true);
        }
    }
}
