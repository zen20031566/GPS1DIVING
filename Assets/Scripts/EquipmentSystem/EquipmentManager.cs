using UnityEngine;
using System.Collections.Generic;

public class EquipmentManager : MonoBehaviour
{
    private Dictionary<string, EquipmentSO> allEquipmentsMap;

    private List<EquipmentSO> playerEquipment;

    //Properties
    public List<EquipmentSO> PlayerEquipment => playerEquipment;

    private void Awake()
    {
        allEquipmentsMap = CreateEquipmentMap();
    }

    private void GiveEquipment(string id)
    {
        playerEquipment.Add(GetEquipmentById(id));
    }

    private EquipmentSO GetEquipmentById(string id)
    {
        EquipmentSO equipment = allEquipmentsMap[id];

        if (equipment == null)
        {
            Debug.LogError("id not found in the equipment map: " + id);
        }

        return equipment;
    }

    private Dictionary<string, EquipmentSO> CreateEquipmentMap()
    {
        EquipmentSO[] allEquipment = Resources.LoadAll<EquipmentSO>("Equipment");

        Dictionary<string, EquipmentSO> idToEquipmentsMap = new Dictionary<string, EquipmentSO>();

        foreach (EquipmentSO equipmentData in allEquipment)
        {
            if (idToEquipmentsMap.ContainsKey(equipmentData.Id))
            {
                Debug.LogWarning("Duplicate id found when creating equipment map: " + equipmentData.Id);
                continue; //Skip this equipment to avoid duplicates
            }

            idToEquipmentsMap.Add(equipmentData.Id, equipmentData);
        }

        return idToEquipmentsMap;
    }
}
