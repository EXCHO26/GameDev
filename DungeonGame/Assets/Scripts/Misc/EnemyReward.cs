using UnityEngine;

public class EnemyReward : MonoBehaviour
{
    [Header("Inventory Setup")]
    [SerializeField] private InventoryModel playerInventory;

    [Header("Reward Setup")]
    [SerializeField] private ResourceData rewardType;
    [SerializeField] private int minAmount = 1;
    [SerializeField] private int maxAmount = 5;

    public void GiveRewardToPlayer()
    {
        if (playerInventory == null || rewardType == null)
        {
            return;
        }

        int randomAmount = Random.Range(minAmount, maxAmount + 1);

        playerInventory.AddResource(rewardType, randomAmount);
    }
}
