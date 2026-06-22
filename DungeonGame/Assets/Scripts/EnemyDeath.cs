using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public void HandleDeath()
    {
        if (TryGetComponent<EntityController>(out var controller)) controller.enabled = false;
        if (TryGetComponent<Rigidbody2D>(out var rb)) rb.linearVelocity = Vector2.zero;
        if (TryGetComponent<Collider2D>(out var col)) col.enabled = false;
        if (TryGetComponent<EnemyReward>(out var reward)) reward.GiveRewardToPlayer();

        Destroy(gameObject, 2f);
    }
}