using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Skills.Effects
{

    [CreateAssetMenu(fileName = "N [Effect]",
        menuName = "Combat/Effect/Team/Control")]
    public class STeamControlEffect : SEffect
    {
        [SerializeField] private bool isBurst;
        public override void DoEffect(in CombatEntity performer, in CombatEntity target, in float effectValue)
        {
            var targetTeam = target.Team;
            bool isAlly = UtilsTeam.IsAllyEntity(in performer, in targetTeam);
            if (!isAlly)
            {
                //todo make enemy Control variation
            }

            if (isBurst)
            {
                UtilsCombatTeam.BurstControl(in targetTeam,in effectValue);
            }
            else
            {
                UtilsCombatTeam.GainControl(in targetTeam, in effectValue);
            }
        }


        [Button]
        protected void UpdateAssetName()
        {
            string header = "Team CONTROL - ";
            string body;
            string suffix = " [Effect]";

            body = isBurst ? "Burst" : "Gain";

            string generatedName = header + body + suffix;
            UtilsAssets.UpdateAssetName(this, generatedName);
        }
    }
}
