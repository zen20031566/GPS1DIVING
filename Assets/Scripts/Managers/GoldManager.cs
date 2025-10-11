using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    [SerializeField] private int startingGold = 0;
    [SerializeField] private TMP_Text goldText;

    private int goldAmount;

    //Properties
    public int GoldAmount => goldAmount;

    private void OnEnable()
    {
        GameEventsManager.Instance.GoldEvents.OnGoldAdd += AddGold;
        GameEventsManager.Instance.GoldEvents.OnGoldDecrease += DecreaseGold;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.GoldEvents.OnGoldAdd -= AddGold;
        GameEventsManager.Instance.GoldEvents.OnGoldDecrease -= DecreaseGold;
    }

    private void Start()
    {
        goldAmount = startingGold;
        goldText.text = goldAmount + " gold";
        GameEventsManager.Instance.GoldEvents.GoldChange(goldAmount); 
    }

    private void AddGold(int amount)
    {
        goldAmount += amount;
        GameEventsManager.Instance.GoldEvents.GoldChange(goldAmount);
        goldText.text = goldAmount + " gold";
    }

    private void DecreaseGold(int amount)
    {
        goldAmount -= amount;
        GameEventsManager.Instance.GoldEvents.GoldChange(goldAmount);
        goldText.text = goldAmount + " gold";
    }

}
