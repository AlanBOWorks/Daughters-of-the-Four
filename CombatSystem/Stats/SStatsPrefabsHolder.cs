using System;
using CombatSystem.Skills.Effects;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Stats
{

    [CreateAssetMenu(fileName = "PREFAB - N [StatsType Holder]",
        menuName = "Combat/Holders/Stats Type/Prefabs [Holder]")]
    public class SStatsPrefabsHolder : ScriptableObject
    {
        [SerializeField]
        private ReferenceHolder holder = new ReferenceHolder();

        public IFullEffectStructureRead<GameObject> GetHolder() => holder;
        internal ReferenceHolder GetReferences() => holder;


        [Serializable]
        internal sealed class ReferenceHolder : MonoEffectStructure<GameObject>
        {
            
        }

        private const string AssetName = "PREFAB PARTICLES - Player [StatsType Holder]";
        public const string EditorsName = AssetName + ".asset";
        [Button]
        private void UpdateAssetNameToEditorsName()
        {
            UtilsAssets.UpdateAssetName(this, AssetName);
        }
    }
}
