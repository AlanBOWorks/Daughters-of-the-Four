using ___ProjectExclusive;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Skills
{
    public abstract class SSkillUseConditionBase : ScriptableObject
    {
        public abstract bool CanUseSkill(CombatingEntity user, float conditionalCheck);

        protected const string SkillUseConditionPrefix = " - [Skill Use Condition]";

        protected virtual string GenerateAssetName()
        {
            return SkillUseConditionPrefix;
        }

        [Button(ButtonSizes.Large)]
        private void UpdateAssetName()
        {
            name = GenerateAssetName();
            UtilsGame.UpdateAssetName(this);
        }
    }
}
