using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    public static class UtilsEffectOrganization 
    {
        public static EnumsEffect.Archetype ConvertEffectArchetype(in IEffect effect)
        {
            return effect switch
            {
                IOffensiveEffect _ => EnumsEffect.Archetype.Offensive,
                ITeamEffect _ => EnumsEffect.Archetype.Team,
                ISupportEffect _ => EnumsEffect.Archetype.Support,
                _ => EnumsEffect.Archetype.Others
            };
        }
    }
}
