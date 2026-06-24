using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero Data", menuName = "Character/Hero Data")]
public class HeroData : ScriptableObject
{
    [Header("Hero Info")]
    public string heroName;

    [Header("Basic Attack")]
    public AbilityBase basicAttackAsset;
    
    [Header("Starting Abilities")]
    public List<AbilityBase> startingAbilities; 
}
