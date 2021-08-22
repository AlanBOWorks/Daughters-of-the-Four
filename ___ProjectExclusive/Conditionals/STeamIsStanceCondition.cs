using ___ProjectExclusive;
using _Team;
using Characters;
using CombatConditions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "TEAM isStance - N [Condition]",
        menuName = "Combat/Conditions/Team Is Stance")]
    public class STeamIsStanceCondition : SCondition
    {
        [SerializeField] private TeamCombatState.Stances checkStance =
            TeamCombatState.Stances.Neutral;

        public override bool CanApply(CombatingEntity target, float checkValue)
        {
            var userTeam = target.CharacterGroup.Team;
            return userTeam.State.CurrentStance == checkStance;
        }

        [Button(ButtonSizes.Large), GUIColor(.3f, .6f, 1f)]
        private void UpdateAssetName()
        {
            name = $"TEAM isStance - {checkStance.ToString().ToUpper()} [Condition]";
            UtilsGame.UpdateAssetName(this);
        }
    }
}
