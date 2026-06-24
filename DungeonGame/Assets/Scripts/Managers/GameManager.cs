using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("UI References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private DeadMenu deadMenu;

    [Header("Inventory Settings")]
    public InventoryModel inventory;
    public List<ResourceData> allPossibleResources;

    [Header("Ability Settings")]
    public AbilityController abilityController;

    private bool isPaused = false;
    private bool isPlayerDead = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (SaveManager.isFirstTimeLoading)
        {
            LoadGameState();
            SaveManager.isFirstTimeLoading = false;
        }
    }

    private void Update()
    {
        if (!isPlayerDead && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void OnPlayerDeath()
    {
        isPlayerDead = true;
        if (deadMenu)
        {
            deadMenu.ShowMenuSmoothly();
        }
    }

    public void RespawnPlayer()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartingHub");
    }

    public void HandleExitToMenu()
    {
        Time.timeScale = 1f;
        SaveGameState();
        SceneManager.LoadScene("MainMenu");
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
    }

    public void SaveGameState()
    {
        GameSaveData data = new GameSaveData();

        if (inventory != null)
        {
            data.inventoryResources = inventory.GetInventorySaveData();
        }

        if (abilityController != null)
        {
            data.unlockedAbilities = abilityController.GetAbilitySaveData();
        }

        SaveManager.SaveGame(data);
    }

    public void LoadGameState()
    {
        GameSaveData data = SaveManager.LoadGame();

        if (inventory != null)
        {
            inventory.LoadSavedInventory(data.inventoryResources, allPossibleResources);
        }

        if (abilityController != null)
        {
            abilityController.LoadSavedAbilities(data.unlockedAbilities);
        }
    }

    private void OnApplicationQuit()
    {
        SaveGameState();
    }
}