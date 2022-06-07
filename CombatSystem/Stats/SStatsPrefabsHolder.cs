using System;
using UnityEngine;

namespace CombatSystem.Stats
{

    [CreateAssetMenu(fileName = "PREFAB - N [StatsType Holder]",
        menuName = "Combat/Holders/Stats Type/Prefabs [Holder]")]
    public class SStatsPrefabsHolder : ScriptableObject
    {
        [SerializeField]
        private ReferenceHolder holder = new ReferenceHolder();

        public IStatsRead<GameObject> GetHolder() => holder;
        internal ReferenceHolder GetReferences() => holder;


        [Serializable]
        internal sealed class ReferenceHolder : MonoStatsStructure<GameObject>
        {
            
        }
    }
}