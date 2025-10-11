using UnityEngine;

public class GearUpgradeGroup : MonoBehaviour
{
    [SerializeField] private GearUpgradeTab tabPrefab;
    private Player player;

    private void Initialize(Player player)
    {
        this.player = player;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void InstiantiateUpgradeTab()
    {
        GearUpgradeTab GearUpgradeTab = Instantiate(tabPrefab, gameObject.transform);
    }
}
