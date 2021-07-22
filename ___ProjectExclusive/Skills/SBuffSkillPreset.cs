using System;
using System.Collections.Generic;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "N (T) - BUFF Skill L - [Preset]",
        menuName = "Combat/Buff Preset")]
    public class SBuffSkillPreset : SSkillPresetBase
    {
        [TitleGroup("Effects")]
        [SerializeField] private List<BuffParams> buffs = new List<BuffParams>(1);
        public override IEffect GetEffect(int index) => buffs[index];
        public override int GetEffectAmount() => buffs.Count;
    }

    [Serializable]
    public class BuffParams : EffectParamsBase
    {
        [Title("Preset")] 
        public SEffectBuffBase buffPreset;

        [SerializeField] 
        private bool isBurstType;
        public bool IsBurstType => isBurstType;


        public override void DoEffect(CombatingEntity user, CombatingEntity target, float randomModifier)
        {
            buffPreset.DoEffect(user,target,isBurstType, power * randomModifier);
        }
    }
}
