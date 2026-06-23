using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Ability", menuName = "Abilities/Heal")]
public class HealAbility : AbilityBase
{
    public override void Activate(GameObject caster, int currentLevel)
    {
        int dataIndex = currentLevel - 1;
        
        if (dataIndex < 0 || dataIndex >= levels.Length) 
        {
            return;
        }

        AbilityLevelData currentData = levels[dataIndex];
        Health heroHealth = caster.GetComponent<Health>();

        if (heroHealth)
        {
            heroHealth.AddHealth(currentData.effectAmount);
        }
    }
}
