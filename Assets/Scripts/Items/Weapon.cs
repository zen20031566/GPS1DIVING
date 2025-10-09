using UnityEngine;

public class Weapon : Item, IEquipable
{
    public void Equip()
    {
        Debug.Log("Equpped item " + this.itemDataSO.DisplayName);
    }
    public void Unequip()
    {
        Debug.Log("Unequipped item " + this.itemDataSO.DisplayName);
    }

}
