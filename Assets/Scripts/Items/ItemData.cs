
public class ItemData 
{
    public ItemDataSO ItemDataSO;

    public ItemData (ItemDataSO itemDataSO)
    {
        this.ItemDataSO = itemDataSO;
    }

    public bool HasItemTag(ItemTag tagToFind)
    {
        if (this.ItemDataSO.Tags.Count == 0) return false;

        foreach (ItemTag tag in this.ItemDataSO.Tags)
        {
            if (tag == tagToFind) return true;
        }

        return false;
    }
}
