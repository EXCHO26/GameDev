using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Sprint Ability", menuName = "Abilities/Sprint")]
public class SprintAbility : AbilityBase
{
    public override void Activate(GameObject caster, int currentLevel)
    {
        AbilityLevelData currentData = levels[currentLevel - 1];

        EntityController movement = caster.GetComponent<EntityController>();
        
        AbilityController controller = caster.GetComponent<AbilityController>();

        if (movement && controller)
        {
            controller.StartCoroutine(SprintRoutine(movement, currentData.effectAmount, currentData.duration));
        }
    }

    private IEnumerator SprintRoutine(EntityController movement, float speedBonus, float duration)
    {
        movement.ModifySpeed(speedBonus);

        yield return new WaitForSeconds(duration);
        
        movement.ModifySpeed(-speedBonus);
    }
}