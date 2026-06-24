using UnityEngine;
using System.Collections;

public class EnemyDeath : MonoBehaviour
{
    [HideInInspector] public EntitySpawner spawner;
    [HideInInspector] public GameObject prefabOrigin;

    private Collider2D enemyCollider; 
    
    private EnemyController enemyController;

    private Rigidbody2D rb;
    
    private void Awake()
    {
        enemyCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        enemyController = GetComponent<EnemyController>();
    }

    public void HandleDeath()
    {
        if (enemyCollider) enemyCollider.enabled = false;

        if (enemyController) enemyController.enabled = false;

        if (rb) rb.linearVelocity = Vector2.zero;

        if (TryGetComponent<EnemyReward>(out var reward)) reward.GiveRewardToPlayer();

        if (spawner && prefabOrigin) StartCoroutine(ReturnToPoolAfterDelay(2f));

        else Destroy(gameObject, 2f);
    }

    public void ResetEnemy()
    {
        if (enemyCollider) enemyCollider.enabled = true;
        if (enemyController) enemyController.enabled = true;
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        spawner.ReturnEnemyToPool(gameObject, prefabOrigin);
    }
}