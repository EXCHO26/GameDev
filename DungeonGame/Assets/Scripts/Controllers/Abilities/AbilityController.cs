using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveAbility
{
    public AbilityBase ability;
    public int currentLevel = 1;
    public float lastUsedTime;

    public ActiveAbility(AbilityBase abilityAsset)
    {
        ability = abilityAsset;
        currentLevel = 1;
        lastUsedTime = -100f;
    }

    public bool TryUpgrade(InventoryModel playerInventory) 
    {
        if (currentLevel >= ability.levels.Length)
        {
            return false;
        }

        UpgradeCost[] nextLevelCosts = ability.levels[currentLevel].upgradeCosts;
        if (playerInventory.HasEnoughResources(nextLevelCosts))
        {
            playerInventory.ConsumeResources(nextLevelCosts);
            currentLevel++;

            return true;
        }
        else
        {
            return false;
        }
    }
}

public class AbilityController : MonoBehaviour
{
    [Header("Hero Data")]
    public HeroData myHeroData;

    [HideInInspector]
    public ActiveAbility basicAttack;

    [HideInInspector]
    public List<ActiveAbility> abilities = new List<ActiveAbility>();

    [Header("UI")]
    public AbilityUI abilitySlot1UI;
    public AbilityUI abilitySlot2UI;

    void Start()
    {
        FillAbilities();
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && basicAttack != null)
        {
            TryUseAbility(basicAttack);
        }

        if (Input.GetKeyDown(KeyCode.Q) && abilities.Count > 0)
        {
            TryUseAbility(abilities[0]);
        }

        if (Input.GetKeyDown(KeyCode.E) && abilities.Count > 1)
        {
            TryUseAbility(abilities[1]);
        }


    }

    public void UpdateUI()
    {
        if (abilities.Count >= 2)
        {
            if (abilitySlot1UI) abilitySlot1UI.Setup(abilities[0]);
            if (abilitySlot2UI) abilitySlot2UI.Setup(abilities[1]);
        }
    }
    
    public void FillAbilities()
    {
        if (myHeroData)
        {
            abilities.Clear();

            if (myHeroData.basicAttackAsset != null)
            {
                basicAttack = new ActiveAbility(myHeroData.basicAttackAsset);
            }

            foreach (var abilityAsset in myHeroData.startingAbilities)
            {
                abilities.Add(new ActiveAbility(abilityAsset));
            }
        }
    }

    void TryUseAbility(ActiveAbility activeAbility)
    {
        float cooldown = activeAbility.ability.levels[activeAbility.currentLevel - 1].cooldown;

        if (Time.time >= activeAbility.lastUsedTime + cooldown)
        {
            activeAbility.ability.Activate(gameObject, activeAbility.currentLevel);
            activeAbility.lastUsedTime = Time.time;
        }
    }

    public List<AbilitySaveData> GetAbilitySaveData()
    {
        List<AbilitySaveData> saveDataList = new List<AbilitySaveData>();

        if (basicAttack != null && basicAttack.ability != null)
        {
            saveDataList.Add(new AbilitySaveData { 
                abilityName = basicAttack.ability.name, 
                currentLevel = basicAttack.currentLevel 
            });
        }

        if (abilities != null)
        {
            foreach (ActiveAbility ability in abilities)
            {
                if (ability != null && ability.ability != null)
                {
                    saveDataList.Add(new AbilitySaveData { 
                        abilityName = ability.ability.name, 
                        currentLevel = ability.currentLevel 
                    });
                }
            }
        }

        return saveDataList;
    }
    
    public void LoadSavedAbilities(List<AbilitySaveData> savedAbilities)
    {
        FillAbilities();

        foreach (AbilitySaveData saved in savedAbilities)
        {
            if (basicAttack != null && basicAttack.ability != null && basicAttack.ability.name == saved.abilityName)
            {
                basicAttack.currentLevel = saved.currentLevel;
                continue;
            }

            if (abilities != null)
            {
                for (int i = 0; i < abilities.Count; i++)
                {
                    if (abilities[i] != null && abilities[i].ability != null && abilities[i].ability.name == saved.abilityName)
                    {
                        abilities[i].currentLevel = saved.currentLevel;
                        break;
                    }
                }
            }
        }

        UpdateUI();
    }
}