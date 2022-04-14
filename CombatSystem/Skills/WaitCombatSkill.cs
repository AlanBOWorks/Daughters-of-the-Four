using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills
{
    public sealed class WaitCombatSkill : CombatSkill
    {
        [ShowInInspector]
        private static readonly WaitSkillConstruction GlobalInstantiation = new WaitSkillConstruction();
        public WaitCombatSkill() : base(GlobalInstantiation)
        {
        }

        private sealed class WaitSkillConstruction : IFullSkill
        {
            public int SkillCost => 0;
            public EnumsSkill.Archetype Archetype => EnumsSkill.Archetype.Self;
            public EnumsSkill.TargetType TargetType => EnumsSkill.TargetType.Direct;

            public void DoSkill(in CombatEntity performer, in CombatEntity target, in CombatSkill holderReference)
            {
                UtilsCombatStats.FullTickActions(target.Stats);
                CombatSystemSingleton.EventsHolder.OnEntityWaitSequence(target);
            }

            private const string WaitSkillName = "Wait";
            public string GetSkillName()
            {
                return WaitSkillName;
            }

            public Sprite GetSkillIcon()
            {
                return null;
            }
        }
    }
}
