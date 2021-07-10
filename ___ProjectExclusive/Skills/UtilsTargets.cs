using System;
using System.Collections.Generic;
using _CombatSystem;
using Characters;
using UnityEngine;

namespace Skills
{
    public static class UtilsTargets
    {
        public static List<CombatingEntity> GetEffectTargets(EffectParams effect, CombatingEntity target)
        {
            List<CombatingEntity> applyEffectOn;
            SEffectBase.EffectTarget targetType = effect.GetEffectTarget();
            if (targetType == SEffectBase.EffectTarget.All)
                return CombatSystemSingleton.Characters.AllEntities;

            CharacterSelfGroup targetGroup = target.CharacterGroup;
            switch (targetType)
            {
                case SEffectBase.EffectTarget.Target:
                    applyEffectOn = targetGroup.Self;
                    break;
                case SEffectBase.EffectTarget.TargetTeam:
                    applyEffectOn = targetGroup.Team;
                    break;
                case SEffectBase.EffectTarget.TargetTeamExcluded:
                    applyEffectOn = targetGroup.TeamNotSelf;
                    break;
                default:
                    throw new ArgumentException($"Target type is not defined: {(int) targetType}");
            }

            return applyEffectOn;
        }
    }
}
