using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityListButton : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    
    private ActiveAbility myAbility;
    private UpgradeMenu mainUI;

    public void Setup(ActiveAbility ability, UpgradeMenu ui)
    {
        myAbility = ability;
        mainUI = ui;

        // 1. Проверяваме дали магията изобщо съществува в слота на играча
        if (ability == null || ability.ability == null)
        {
            Debug.LogError("Внимание: Имаш празен слот за магия в AbilityController-а на играча!");
            return;
        }

        // 2. Проверяваме дали си свързал картинката и текста в префаба
        if (icon == null || nameText == null)
        {
            Debug.LogError("Внимание: Забравил си да плъзнеш Icon или NameText в Инспектора на префаба AbilityButton!");
            return;
        }
        
        icon.sprite = ability.ability.icon;
        nameText.text = ability.ability.name;
    }

    public void OnButtonClicked()
    {
        mainUI.SelectAbility(myAbility);
    }
}