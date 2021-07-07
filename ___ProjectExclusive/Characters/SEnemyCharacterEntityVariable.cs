using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(fileName = "_Enemy Entity_ - N [Enemy Variable]",
        menuName = "Variable/Enemy/Character Entity")]
    public class SEnemyCharacterEntityVariable : SCharacterEntityVariable
    {
        [Title("Stats")]
        [SerializeField]
        private CharacterCombatStatsFull presetStats = new CharacterCombatStatsFull();

        public override CharacterCombatData GenerateData()
        {
            return new CharacterCombatData(presetStats);
        }
    }
}
