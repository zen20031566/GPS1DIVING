using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public bool CanInteract { get; set; } = true;
    [SerializeField] private ItemDataSO itemData;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D col;

    //Properties
    public ItemDataSO ItemData => itemData;

    private void Start()
    {
        InitializeItem(itemData);
    }

    public void InitializeItem(ItemDataSO itemData)
    {
        this.itemData = itemData;
        spriteRenderer.sprite = itemData.DisplaySprite;
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;
        col.size = new Vector2(spriteWidth, spriteHeight);
    }

    public void Interact(Player player)
    {
        if (player.inventoryManager.InventoryGrid.CheckHasEmptySlot() == true)
        {
            Debug.Log("GFood");
            player.inventoryManager.AddItem(itemData);

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Fail");
            //Inventory full
        }
    }
}
