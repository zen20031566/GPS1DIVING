using UnityEngine;
using UnityEngine.UI;

public class PlayerEquipment : MonoBehaviour
{
    //Can change to events later idk not much dif can be better for sound later on
    //Weapon
    private const int numberOfEquipmentSlots = 5;
    [SerializeField] private Image[] EquipmentSlotDisplays = new Image[numberOfEquipmentSlots];
    public Item[] EquipedItems = new Item[numberOfEquipmentSlots];

    [SerializeField] private Transform equipedItemTransform;

    private int selectedEquipmentIndex = 0;

    Player player;
    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (player.PlayerStateMachine.CurrentState != player.OnUIOrDialog && equipedItemTransform.childCount > 0)
        {
            if (InputManager.ScrollDirection > 0)
            {
                selectedEquipmentIndex = (selectedEquipmentIndex - 1 + equipedItemTransform.childCount) % equipedItemTransform.childCount;
                SelectEquipment();
            }
            else if (InputManager.ScrollDirection < 0)
            {
                selectedEquipmentIndex = (selectedEquipmentIndex + 1) % equipedItemTransform.childCount;
                SelectEquipment();
            }
        }
        
    }

    public void SelectEquipment()
    {
        int i = 0;

        foreach (Transform child in equipedItemTransform)
        {
            if (i == selectedEquipmentIndex)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
                i++;
        }

    }

    public void InstantiateEquipment(ItemData itemData, int slot)
    {
        if (itemData == null) return;

        Item item = Instantiate(itemData.ItemDataSO.Prefab, equipedItemTransform.position, Quaternion.identity, equipedItemTransform);
        item.InitializeItem(itemData.ItemDataSO);
        item.rb.simulated = false;
        EquipedItems[slot] = item;
        item.gameObject.SetActive(false);
        EquipmentSlotDisplays[slot].sprite = itemData.ItemDataSO.InventorySprite;
        EquipmentSlotDisplays[slot].color = new Color(1, 1, 1, 1);

        //if (item is IEquipable equipableItem)
        //{
        //    equipableItem.Equip();
        //}
    }

    public void RemoveEquipment(int slot)
    {
        if (slot < EquipedItems.Length)
        {
            Destroy(EquipedItems[slot].gameObject);
            EquipedItems[slot] = null;
            EquipmentSlotDisplays[slot].sprite = null;
            EquipmentSlotDisplays[slot].color = new Color(1, 1, 1, 0);
        }
    }

}
