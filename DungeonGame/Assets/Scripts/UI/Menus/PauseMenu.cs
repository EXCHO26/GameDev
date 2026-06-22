using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0f; 
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
    
    public void Resume()
    {
        gameObject.SetActive(false);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        GameManager.instance.HandleExitToMenu();
    }
}