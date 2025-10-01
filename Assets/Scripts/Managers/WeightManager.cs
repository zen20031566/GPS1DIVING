using UnityEngine;

public class WeightManager : MonoBehaviour
{
    [SerializeField] private float maxWeight;
    private float currentWeight = 0f;

    //Properties
    public float MaxWeight => maxWeight;
    public float CurrentWeight => currentWeight;    

    private void OnEnable()
    {
        GameEventsManager.Instance.TrashEvents.OnTrashCollected += TrashCollected;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.TrashEvents.OnTrashCollected -= TrashCollected;
    }

   private void TrashCollected(TrashSO trashData)
   {
        currentWeight += trashData.Weight;
        GameEventsManager.Instance.WeightEvents.WeightChange(currentWeight);
    }
}

