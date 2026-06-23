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