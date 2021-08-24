using System;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(fileName = "_Enemy Entity_ - N [Enemy Variable]",
        menuName = "Variable/Enemy/Character Entity")]
    public class SEnemyCharacterEntityVariable : SCharacterEntityVariable
    {
        [TitleGroup("Stats")]
        [SerializeField]
        private CombatStatsFull presetStats = new CombatStatsFull();

        public override CombatStatsHolder GenerateCombatData()
        {
            return new CombatStatsHolder(presetStats);
        }
    }
}
