using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UpgradeMenu : MonoBehaviour
{
    public InventoryModel playerInventory; 
    public AbilityController playerAbilities;

    [Header("Left Side (Ability List)")]
    public Transform contentTransform;
    public GameObject abilityButtonPrefab;

    [Header("Right Side (Details)")]
    public TextMeshProUGUI skillNameText;
    public Image skillIcon;
    public TextMeshProUGUI levelText;
    public Button upgradeButton;
    
    [Header("Resources")]
    public Transform costContainer;
    public GameObject resourceCostPrefab;

    private ActiveAbility selectedAbility;

    private void OnEnable()
    {
        GenerateAbilityList();
    }

    private void GenerateAbilityList()
    {
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        foreach (ActiveAbility ability in playerAbilities.abilities)
        {
            if (ability == null || ability.ability == null) continue;

            GameObject newBtn = Instantiate(abilityButtonPrefab, contentTransform, false);
            newBtn.GetComponent<AbilityListButton>().Setup(ability, this);
        }
        
        if (playerAbilities.basicAttack != null && playerAbilities.basicAttack.ability != null)
        {
            GameObject atkBtn = Instantiate(abilityButtonPrefab, contentTransform, false);
            atkBtn.GetComponent<AbilityListButton>().Setup(playerAbilities.basicAttack, this);
            SelectAbility(playerAbilities.basicAttack);
        }
    }

    public void SelectAbility(ActiveAbility ability)
    {
        selectedAbility = ability;
        UpdateUIDetails();
    }

    private void UpdateUIDetails()
    {
        if (selectedAbility == null) return;

        UpdateBasicInfo();
        ClearCostContainer();

        if (IsAbilityMaxLevel())
        {
            SetupMaxLevelUI();
        }
        else
        {
            SetupUpgradeUI();
        }
    }

    private void UpdateBasicInfo()
    {
        skillNameText.text = selectedAbility.ability.name;
        if (selectedAbility.ability.icon != null)
        {
            skillIcon.sprite = selectedAbility.ability.icon;
        }
    }

    private void ClearCostContainer()
    {
        foreach (Transform child in costContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private bool IsAbilityMaxLevel()
    {
        return selectedAbility.currentLevel >= selectedAbility.ability.levels.Length;
    }

    private void SetupMaxLevelUI()
    {
        levelText.text = "Level: MAX";
        upgradeButton.interactable = false;
    }

    private void SetupUpgradeUI()
    {
        levelText.text = $"Level: {selectedAbility.currentLevel} -> {selectedAbility.currentLevel + 1}";
        
        UpgradeCost[] costs = selectedAbility.ability.levels[selectedAbility.currentLevel].upgradeCosts;
        
        GenerateResourceCostsUI(costs);

        upgradeButton.interactable = playerInventory.HasEnoughResources(costs);
    }

    private void GenerateResourceCostsUI(UpgradeCost[] costs)
    {
        foreach (var cost in costs)
        {
            GameObject newCostUI = Instantiate(resourceCostPrefab, costContainer, false);
            
            Image resIcon = newCostUI.transform.Find("ResourceImage").GetComponent<Image>();
            TextMeshProUGUI resText = newCostUI.transform.Find("ResourceQuantity").GetComponent<TextMeshProUGUI>();
            
            if (cost.resource.icon != null)
            {
                resIcon.sprite = cost.resource.icon;
            }
            
            int currentAmount = playerInventory.GetResourceCount(cost.resource);
            resText.text = $"{currentAmount} / {cost.amount}";
            
            resText.color = currentAmount >= cost.amount ? Color.white : Color.red;
        }
    }

    public void OnUpgradeButtonClicked()
    {
        if (selectedAbility != null && selectedAbility.TryUpgrade(playerInventory))
        {
            UpdateUIDetails();
        }
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f; 
    }
}