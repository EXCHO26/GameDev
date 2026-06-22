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
        if (amount <= 0) return;

        if (!_resources.ContainsKey(resource))
        {
            _resources[resource] = 0;
        }

        _resources[resource] += amount;
        OnResourceChanged?.Invoke(resource, _resources[resource]);
    }

    public int GetResourceCount(ResourceData resource)
    {
        return _resources.ContainsKey(resource) ? _resources[resource] : 0;
    }

    public void Clear()
    {
        _resources.Clear();
    }
}