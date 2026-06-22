using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryModel inventory;
    [SerializeField] private ResourceData trackedResource;
    [SerializeField] private Text resourceText;

    private void OnEnable()
    {
        inventory.OnResourceChanged += UpdateUI;
    }

    private void OnDisable()
    {
        inventory.OnResourceChanged -= UpdateUI;
    }

    private void Start()
    {
        UpdateText(inventory.GetResourceCount(trackedResource));
    }

    private void UpdateUI(ResourceData resource, int newAmount)
    {
        if (resource == trackedResource)
        {
            UpdateText(newAmount);
        }
    }

    private void UpdateText(int amount)
    {
        resourceText.text = amount.ToString();
    }
}
