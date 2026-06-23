using UnityEngine;
using System.Collections;

public class BossDeath : MonoBehaviour
{
    [Header("Portal Settings")]
    public GameObject portalPrefab;

    [Header("Animation Settings")]
    public float deathDelay = 2f;

    public void Die()
    {
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        EnemyController enemyController = GetComponent<EnemyController>();
        if (enemyController) enemyController.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb) rb.linearVelocity = Vector2.zero;
        
        if (TryGetComponent<EnemyReward>(out var reward)) reward.GiveRewardToPlayer();
        
        yield return new WaitForSeconds(deathDelay);

        SpawnPortal();

        Destroy(gameObject);
    }

    private void SpawnPortal()
    {
        if (portalPrefab)
        {
            GameObject portal = Instantiate(portalPrefab, transform.position, Quaternion.identity);
        }
    }

    
}
