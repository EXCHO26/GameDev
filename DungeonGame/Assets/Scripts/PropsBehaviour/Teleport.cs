using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [Header("Settings")]
    public string sceneToLoad;
    public float waitTime = 2.0f;

    private float timer = 0f;
    private bool playerInside = false;

    private void Update()
    {
        if (playerInside)
        {
            timer += Time.deltaTime;

            if (timer >= waitTime)
            {
                TeleportToScene();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("Teleporting in 3 seconds...");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = false;
            timer = 0f;
            Debug.Log("Teleport cancelled.");
        }
    }

    void TeleportToScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
