using System;
using UnityEngine;

namespace CombatSystem.Stats
{
    [CreateAssetMenu(fileName = "ICONS - N [StatsType Holder]",
        menuName = "Combat/Holders/Stats Type/Icons [Holder]")]
    public class SStatsIconsHolder : ScriptableObject
    {
        [SerializeField]
        private ReferenceHolder holder = new ReferenceHolder();

        public IStatsRead<Sprite> GetHolder() => holder;
        internal ReferenceHolder GetReferences() => holder;


        [Serializable]
        internal sealed class ReferenceHolder : PreviewMonoStatsStructure<Sprite>
        {

        }
    }
}
