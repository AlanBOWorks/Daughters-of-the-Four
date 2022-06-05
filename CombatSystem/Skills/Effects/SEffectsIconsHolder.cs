using System;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(fileName = "ICONS - N [EffectsType Holder]",
        menuName = "Combat/Holders/Effect Type/Icons [Holder]")]
    public class SEffectsIconsHolder : ScriptableObject
    {
        [SerializeField]private ReferencesHolder holder = new ReferencesHolder();

        public IEffectStructureRead<Sprite> GetHolder() => holder;

        [Serializable]
        private sealed class ReferencesHolder : PreviewMonoEffectStructure<Sprite>
        {
            
        }
    }
}
