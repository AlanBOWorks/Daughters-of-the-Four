using ___ProjectExclusive;
using Characters;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace Passives
{
    /// <summary>
    /// Auras are Buff/Passives directed towards [<seealso cref="_Team.CombatingTeam"/>] and thus making this the superior type of
    /// stats boots (Since team upgrade are Base type and not Buff type). <br></br>
    /// <br></br>
    /// [<see cref="OpeningPassives"/>] in other hand can simulate Auras but towards [<seealso cref="CombatingEntity"/>],
    /// yet normally those are mean for individuals that a group (but there could be group target), thus making
    /// those a weaker type of buff.
    /// </summary>
    [CreateAssetMenu(fileName = "N"+AuraPassivePrefix, menuName = "Combat/Passive/Aura")]
    public class SAuraPassive : ScriptableObject
    {
        [SerializeField] private string passiveName = "NULL";
        [Title("Stats")]
        [SerializeField]
        private CharacterArchetypes.TeamPosition targetPosition
            = CharacterArchetypes.TeamPosition.MidLine;

        [InfoBox("<b>Passive Stats</b>: These are Team Stats (Base type BUFF)")]
        [SerializeField] private CharacterCombatStatsBasic passiveStats;

        public string PassiveName => passiveName;
        public CharacterArchetypes.TeamPosition InjectionPosition() => targetPosition;
        public ICharacterBasicStats GetStats() => passiveStats;

        private const string AuraPassivePrefix = ") - [Aura Passive]";
        
        [Button(ButtonSizes.Large)]
        private void UpdateAssetName()
        {
            name = passiveName.ToUpper() + " (P: "+ targetPosition + AuraPassivePrefix;
            UtilsGame.UpdateAssetName(this);
        }
    }
}
