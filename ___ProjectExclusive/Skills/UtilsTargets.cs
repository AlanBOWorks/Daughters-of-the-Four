using System.Collections.Generic;
using _CombatSystem;
using Characters;

namespace Skills
{
    public static class UtilsTargets
    {
        
        public static List<CombatingEntity> GetEffectTargets(CombatingEntity target, EffectParams effect)
        {
            List<CombatingEntity> applyEffectOn;
            SEffectBase.EffectTarget targetType = effect.GetEffectTarget();
            if (targetType == SEffectBase.EffectTarget.All)
                return CombatSystemSingleton.Characters.AllEntities;

            CharacterSelfGroup targetGroup = target.CharacterGroup;
            switch (targetType)
            {
                default:
                case SEffectBase.EffectTarget.Target:
                    applyEffectOn = targetGroup.Self;
                    break;
                case SEffectBase.EffectTarget.TargetTeam:
                    applyEffectOn = targetGroup.Team;
                    break;
                case SEffectBase.EffectTarget.TargetTeamExcluded:
                    applyEffectOn = targetGroup.TeamNotSelf;
                    break;
            }

            return applyEffectOn;
        }
    }
}
