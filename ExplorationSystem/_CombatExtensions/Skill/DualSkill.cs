using System.Collections.Generic;
using System.Linq;
using CombatSystem.Skills;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ExplorationSystem
{
    

    public sealed class CraftedDualSkill : IFullSkill
    {
        public CraftedDualSkill()
        {
            _effectValues = new List<PerformEffectValues>();
        }

        private string _skillName;

        [ShowInInspector, DisableInPlayMode]
        private IFullSkill _mainSkill;
        [ShowInInspector, DisableInPlayMode]
        private IFullSkill _secondarySkill;

        [ShowInInspector, DisableInPlayMode]
        private readonly List<PerformEffectValues> _effectValues;

        public void DoInjection(IFullSkill mainSkill, IFullSkill secondarySkill)
        {
            _mainSkill = mainSkill;
            _secondarySkill = secondarySkill;
        
            HandleName();
            HandleCost();
            HandleIgnoreSelf();
            HandleLuck();
            HandleEffects();
        }

        private void HandleName()
        {
            _skillName = _mainSkill.GetSkillName() + "+";
        }
        private void HandleCost()
        {
            float primaryCost = _mainSkill.SkillCost;
            float secondaryCost = _secondarySkill.SkillCost;
            float average = (primaryCost + secondaryCost) * .5f;
            SkillCost = Mathf.FloorToInt(average);
        }

        private void HandleIgnoreSelf()
        {
            IgnoreSelf = _mainSkill.IgnoreSelf && _secondarySkill.IgnoreSelf;
        }

        private void HandleLuck()
        {
            LuckModifier = Mathf.Max(_mainSkill.LuckModifier, _secondarySkill.LuckModifier);
        }

        private void HandleEffects()
        {
            _effectValues.Clear();
            HandleSkills(_mainSkill.GetEffects());
            HandleSkills(_secondarySkill.GetEffects());

            void HandleSkills(IEnumerable<PerformEffectValues> enumerable)
            {
                foreach (var values in enumerable)
                {
                    _effectValues.Add(values);
                }
            }
        }


        public int SkillCost { get; private set; }
        public EnumsSkill.TeamTargeting TeamTargeting => _mainSkill.TeamTargeting;
        public IEffect GetMainEffectArchetype() => _mainSkill.GetMainEffectArchetype();


        public IEnumerable<PerformEffectValues> GetEffects() => _effectValues;

        public bool IgnoreSelf { get; private set; }
        public float LuckModifier { get; private set; }
        public string GetSkillName() => _skillName;
        public Sprite GetSkillIcon() => _mainSkill.GetSkillIcon();

        public IEnumerable<PerformEffectValues> GetEffectsFeedBacks() => _effectValues;
    }
}
