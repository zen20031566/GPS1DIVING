using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Item : MonoBehaviour, IInteractable
{
    public bool CanInteract { get; set; } = true;
    [SerializeField] protected ItemDataSO itemDataSO;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    public BoxCollider2D col;
    public Rigidbody2D rb;

    //Properties
    public ItemDataSO ItemData => itemDataSO;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        InitializeItem(itemDataSO);
    }

    public void InitializeItem(ItemDataSO itemDataSO)
    {
        this.itemDataSO = itemDataSO;
        spriteRenderer.sprite = itemDataSO.DisplaySprite;
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;
        col.size = new Vector2(spriteWidth, spriteHeight);

        switch (itemDataSO.weight)
        {
            case ItemWeight.Light: 
                rb.mass = 5;
                rb.linearDamping = 1;
                break;

            case ItemWeight.Medium: 
                rb.mass = 15;
                rb.linearDamping = 2;
                break;

            case ItemWeight.Heavy:
                rb.mass = 50;
                rb.linearDamping = 5;
                break;

        }
    }

    public void Interact(Player player)
    {
        if (player.inventoryManager.InventoryGrid.CheckHasEmptySlot(itemDataSO) == true)
        {
            Debug.Log("Adding item " + itemDataSO.DisplayName);

            ItemData itemData = new ItemData(itemDataSO);
            player.inventoryManager.AddItem(itemData);

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Failed to add item " + itemDataSO.DisplayName + "Inventory full");
        }
    }
}
