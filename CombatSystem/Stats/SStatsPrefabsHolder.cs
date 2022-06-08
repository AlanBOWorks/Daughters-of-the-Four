using System;
using CombatSystem.Skills.Effects;
using UnityEngine;

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
    }
}
