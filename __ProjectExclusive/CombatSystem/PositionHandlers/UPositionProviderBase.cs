using System;
using CombatTeam;
using UnityEngine;

namespace CombatSystem.PositionHandlers
{
    public abstract class UPositionProviderBase : MonoBehaviour, ICombatTeamsStructureRead<ITeamRoleStructureRead<Transform>> {
        public abstract ITeamRoleStructureRead<Transform> GetPlayerTeam();
        public abstract ITeamRoleStructureRead<Transform> GetEnemyTeam();

        private void Awake()
        {
            CombatSystemSingleton.PositionProvider = this;
        }
    }

    [Serializable]
    public class TeamRoleTransforms : ITeamRoleStructureRead<Transform>
    {
        [SerializeField] private Transform vanguard;
        [SerializeField] private Transform attacker;
        [SerializeField] private Transform support;

        public Transform Vanguard => vanguard;

        public Transform Attacker => attacker;

        public Transform Support => support;
    }
   
}
