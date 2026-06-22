using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private DeadMenu deadMenu;

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
        SceneManager.LoadScene("MainMenu");
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
    }
}