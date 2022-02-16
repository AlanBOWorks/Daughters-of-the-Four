using CombatSystem.Entity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills
{
    public class CombatSkill : IFullSkill
    {
        public CombatSkill(IFullSkill preset)
        {
            Preset = preset;
            SkillCost = preset.SkillCost;
        }

        [ShowInInspector,InlineEditor()]
        public readonly IFullSkill Preset;

        public string GetSkillName() => Preset.GetSkillName();
        public Sprite GetSkillIcon() => Preset.GetSkillIcon();
        [ShowInInspector]
        public int SkillCost { get; private set; }
        public EnumsSkill.Archetype Archetype => Preset.Archetype;
        public EnumsSkill.TargetType TargetType => Preset.TargetType;
        public void DoSkill(in CombatEntity performer, in CombatEntity target, in CombatSkill holderReference)
        {
            Preset.DoSkill(in performer, in target, in holderReference);
        }


        public void DoSkill(in CombatEntity performer, in CombatEntity target) =>
            DoSkill(in performer, in target, this);

        public void IncreaseCost()
        {
            if(SkillCost < 0) return;
            SkillCost++;
        }
        public void ResetCost()
        {
            SkillCost = Preset.SkillCost;
        }
    }
}
