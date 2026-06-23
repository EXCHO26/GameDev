using UnityEngine;

[System.Serializable]
public struct AbilityLevelData
{
    [Tooltip("Points of effect")]
    public int effectAmount;

    [Tooltip("Cooldown in seconds")]
    public float cooldown;

    [Tooltip("Resource cost to use the ability")]
    public float resourceCost;

    [Tooltip("Duration of the ability")]
    public float duration;

    [Header("Upgrade Costs")]
    public UpgradeCost[] upgradeCosts;
}

[System.Serializable]
public struct UpgradeCost
{
    [Tooltip("Upgrade resource")]
    public ResourceData resource; 
    
    [Tooltip("Upgrade quantity")]
    public int amount;
}

public abstract class AbilityBase : ScriptableObject
{
    [Header("Ability Info")]
    public string abilityName;
    public Sprite icon;

    [Header("Ability Levels")]
    public AbilityLevelData[] levels;

    public abstract void Activate(GameObject caster, int currentLevel);
}