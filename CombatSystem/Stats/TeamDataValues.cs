using CombatSystem._Core;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Stats
{
    public sealed class TeamDataValues
    {
        [ShowInInspector]
        public float NaturalControl { get; internal set; }
        [ShowInInspector]
        public float BurstControl { get; internal set; }
        public float CalculateCurrentControl() => NaturalControl + BurstControl;

        [ShowInInspector]
        public EnumTeam.StanceFull CurrentStance;

        public void DoRoundReset()
        {
            BurstControl = 0;
        }
    }
}
