using _Team;
using Characters;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "TEAM Stance Toggle - N [Preset]",
        menuName = "Combat/Effects/Team/Team Stance Toggle")]
    public class SEffectTeamStance : SEffectBase
    {
        [SerializeField] private EnumTeam.Stances targetStance = EnumTeam.Stances.Neutral;

        public override void DoEffect(SkillArguments arguments, CombatingEntity target, float randomCheck = 1)
        {
            DoEffect(target, randomCheck);
        }


        public override void DoEffect(CombatingEntity target, float randomCheck)
        {
            if (FailRandom(randomCheck)) return;

            UtilsArea.TeamToggleStance(target, targetStance);
        }

        [Button(ButtonSizes.Large)]
        private void UpdateAssetName()
        {
            name = $"_TEAM Stance Toggle - {targetStance.ToString().ToUpper()} [Preset]";
            RenameAsset();
        }
    }
}
