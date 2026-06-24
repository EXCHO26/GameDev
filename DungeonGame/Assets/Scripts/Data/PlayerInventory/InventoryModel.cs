using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInventory", menuName = "Inventory/System")]
public class InventoryModel : ScriptableObject
{
    private Dictionary<ResourceData, int> _resources = new Dictionary<ResourceData, int>();

    public Action<ResourceData, int> OnResourceChanged;

    public void AddResource(ResourceData resource, int amount)
    {
        if (amount > 0) ModifyResource(resource, amount);
    }

    public void RemoveResource(ResourceData resource, int amount)
    {
        if (amount > 0) ModifyResource(resource, -amount);
    }

    public int GetResourceCount(ResourceData resource)
    {
        return _resources.ContainsKey(resource) ? _resources[resource] : 0;
    }

    public Dictionary<ResourceData, int> GetAllResources()
    {
        return _resources;
    }

    public void Clear()
    {
        _resources.Clear();
    }

    public bool HasEnoughResources(UpgradeCost[] costs)
    {
        if (costs == null || costs.Length == 0) return true;

        foreach (UpgradeCost cost in costs)
        {
            int amountInInventory = GetResourceCount(cost.resource);

            if (amountInInventory < cost.amount)
            {
                return false;
            }
        }
        return true;
    }

    public void ConsumeResources(UpgradeCost[] costs)
    {
        if (costs == null || costs.Length == 0) return;

        foreach (UpgradeCost cost in costs)
        {
            RemoveResource(cost.resource, cost.amount);
        }
    }

    public List<ResourceSaveData> GetInventorySaveData()
    {
        List<ResourceSaveData> saveDataList = new List<ResourceSaveData>();
        
        foreach (KeyValuePair<ResourceData, int> kvp in _resources)
        {
            if (kvp.Key != null)
            {
                saveDataList.Add(new ResourceSaveData 
                { 
                    resourceName = kvp.Key.name, 
                    amount = kvp.Value 
                });
            }
        }
        
        return saveDataList;
    }

    public void LoadSavedInventory(List<ResourceSaveData> savedResources, List<ResourceData> allPossibleResources)
    {
        Clear();

        foreach (ResourceSaveData savedRes in savedResources)
        {
            ResourceData matchingResource = allPossibleResources.Find(r => r != null && r.name == savedRes.resourceName);
            
            if (matchingResource != null)
            {
                AddResource(matchingResource, savedRes.amount);
            }
        }
    }
    
    private void ModifyResource(ResourceData resource, int amount)
    {
        if (!_resources.ContainsKey(resource))
        {
            _resources[resource] = 0;
        }

        _resources[resource] += amount;

        if (_resources[resource] < 0) 
        {
            _resources[resource] = 0;
        }

        OnResourceChanged?.Invoke(resource, _resources[resource]);
    }
}