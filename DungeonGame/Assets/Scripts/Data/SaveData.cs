using System.Collections.Generic;

[System.Serializable]
public class ResourceSaveData
{
    public string resourceName;
    public int amount;
}

[System.Serializable]
public class AbilitySaveData
{
    public string abilityName;
    public int currentLevel;
}

[System.Serializable]
public class GameSaveData
{
    public List<ResourceSaveData> inventoryResources = new List<ResourceSaveData>();
    public List<AbilitySaveData> unlockedAbilities = new List<AbilitySaveData>();
}