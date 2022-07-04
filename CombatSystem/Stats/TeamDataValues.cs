using CombatSystem._Core;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Stats
{
    public sealed class TeamDataValues
    {
        [ShowInInspector]
        public float CurrentControl { get; internal set; }

        [ShowInInspector]
        public EnumTeam.StanceFull CurrentStance;

        public void DoRoundReset()
        {
            CurrentControl = 0;
        }
    }
}
