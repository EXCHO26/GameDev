using UnityEngine;

public class StatueInteraction : MonoBehaviour
{
    [Header("UI")]
    public GameObject upgradeMenu; 
    public GameObject interactPrompt;
    private KeyCode interactKey = KeyCode.X;

    private bool isPlayerNear = false;

    private void Start()
    {
        if (interactPrompt)
        {
            interactPrompt.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(interactKey))
        {
            OpenMenu();
        }
    }

    private void OpenMenu()
    {
        if (upgradeMenu)
        {
            upgradeMenu.SetActive(true);

            if (interactPrompt)
            {
                interactPrompt.SetActive(false);
            }
            
            Time.timeScale = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;

            if (interactPrompt != null && (upgradeMenu == null || !upgradeMenu.activeSelf))
            {
                interactPrompt.SetActive(true);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            
            if (interactPrompt)
            {
                interactPrompt.SetActive(false);
            }
        }
    }
}