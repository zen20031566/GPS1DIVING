
public class ItemData 
{
    public ItemDataSO ItemDataSO;

    public ItemData (ItemDataSO itemDataSO)
    {
        this.ItemDataSO = itemDataSO;
    }

    public bool HasItemTag(ItemTag tagToFind)
    {
        if (this.ItemDataSO.tags.Count == 0) return false;

        foreach (ItemTag tag in this.ItemDataSO.tags)
        {
            if (tag == tagToFind) return true;
        }

        return false;
    }
}
