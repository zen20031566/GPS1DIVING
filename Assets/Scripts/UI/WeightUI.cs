using TMPro;
using UnityEngine;

public class WeightUI : MonoBehaviour
{
    [SerializeField] private WeightManager weightManager;
    [SerializeField] private TMP_Text weightText;

    private void OnEnable()
    {
        GameEventsManager.Instance.WeightEvents.OnWeightChange += WeightChange;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.WeightEvents.OnWeightChange -= WeightChange;
    }

    private void Start()
    {
        weightText.text = $"Weight: {weightManager.CurrentWeight} / {weightManager.MaxWeight}";
    }

    private void WeightChange(float weight)
    {
        weightText.text = $"Weight: {weight} / {weightManager.MaxWeight}";
    }
}
