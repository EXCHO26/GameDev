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

        if (ability == null || ability.ability == null) return;

        if (icon == null || nameText == null) return;

        icon.sprite = ability.ability.icon;
        nameText.text = ability.ability.name;
    }

    public void OnButtonClicked()
    {
        mainUI.SelectAbility(myAbility);
    }
}