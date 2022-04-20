using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public abstract class UDualTeamOffStructureInstantiateHandler<T> : UDualTeamStructureInstantiateHandlerBase<T>,
        IOppositionTeamStructureRead<IEnumerator<T>>

        where T : MonoBehaviour
    {
        [Title("References - Player")]
        [SerializeField, HorizontalGroup()] private TeamStructureReferences playerTeamType;
        [Title("References - Enemy")]
        [SerializeField, HorizontalGroup()] private TeamStructureReferences enemyTeamType;

        [Title("Params")]
        [SerializeField, DisableInPlayMode] private bool hidePrefabs = true;



        protected override void InstantiateElements()
        {
            playerTeamType.InstantiateElements();
            enemyTeamType.InstantiateElements();
        }

        protected override void HidePrefabs()
        {
            if (!hidePrefabs) return;
            playerTeamType.HidePrefab();
            enemyTeamType.HidePrefab();
        }


        public IEnumerator<T> PlayerTeamType => playerTeamType;
        public IEnumerator<T> EnemyTeamType => enemyTeamType;

        protected override void IterationTeam(in CombatTeam team, bool isPlayerElement, in IEntityElementInstantiationListener<T>[] callListeners)
        {
            var offMembers = GetStructureMembers(in team);
            IterationValues.IsPlayerElement = isPlayerElement;
            var references = (isPlayerElement) ? playerTeamType : enemyTeamType;

            int notNullIndex = 0;
            int iterationIndex = 0;

            while (offMembers.MoveNext() && references.MoveNext())
            {
                var element = references.Current;
                var member = offMembers.Current;

                IterationValues.NotNullIndex = notNullIndex;
                IterationValues.IterationIndex = iterationIndex;


                foreach (var listener in callListeners)
                {
                    listener.OnIterationCall(in element, in member, in IterationValues);
                }

                iterationIndex++;
                if (member == null) continue;
                notNullIndex++;

                ActiveElementsDictionary.Add(member, element);
            }

            references.activeCount = notNullIndex;

            offMembers.Reset();
            references.Reset();
        }


        private IEnumerator<CombatEntity> GetStructureMembers(in CombatTeam team)
        {
            return team.OffRoleMembers;
        }

        [Serializable]
        private sealed class TeamStructureReferences : TeamOffStructureInstantiateHandler<T>
        {
            
        }
    }
}
