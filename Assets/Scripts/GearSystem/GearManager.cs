using UnityEngine;
using System.Collections.Generic;

public class GearManager : MonoBehaviour
{
    private Dictionary<string, GearSO> allEquipmentsMap;

    private List<GearSO> playerEquipment = new List<GearSO>();

    //Properties
    public List<GearSO> PlayerEquipment => playerEquipment;

    private void Awake()
    {
        allEquipmentsMap = CreateEquipmentMap();
    }

    private void Start()
    {
        GiveEquipment("EQUIPMENT_01");

        foreach (GearSO equipment in playerEquipment)
        {
            Debug.Log(equipment.Id);
        }
    }

    private void GiveEquipment(string id)
    {
        playerEquipment.Add(GetEquipmentById(id));
    }

    private GearSO GetEquipmentById(string id)
    {
        GearSO equipment = allEquipmentsMap[id];

        if (equipment == null)
        {
            Debug.LogError("id not found in the equipment map: " + id);
        }

        return equipment;
    }

    private Dictionary<string, GearSO> CreateEquipmentMap()
    {
        GearSO[] allEquipment = Resources.LoadAll<GearSO>("Equipment");

        Dictionary<string, GearSO> idToEquipmentsMap = new Dictionary<string, GearSO>();

        foreach (GearSO equipmentData in allEquipment)
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
