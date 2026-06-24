using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damage = 10;
    [SerializeField] private string targetTag;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            if (other.TryGetComponent<IDamageable>(out IDamageable victim))
            {
                victim.TakeDamage(damage);
            }
        }
    }
}
