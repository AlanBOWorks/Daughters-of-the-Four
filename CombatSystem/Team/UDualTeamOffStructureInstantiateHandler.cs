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


        public IEnumerator<T> PlayerTeamType => playerTeamType.GetEnumerator();
        public IEnumerator<T> EnemyTeamType => enemyTeamType.GetEnumerator();

        protected override void IterationTeam(in CombatTeam team, bool isPlayerElement, in IEntityElementInstantiationListener<T>[] callListeners)
        {
            IterationValues.IsPlayerElement = isPlayerElement;
            var references = (isPlayerElement) ? playerTeamType : enemyTeamType;
            ITeamOffStructureRead<CombatEntity> offMembers = team.GetOffMembersStructure();

            var enumerable = UtilsTeam.GetEnumerable(offMembers, references);
            int notNullIndex = 0;
            int iterationIndex = 0;

            foreach (var pair in enumerable)
            {
                var member = pair.Key;
                var element = pair.Value;
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
        }



        [Serializable]
        private sealed class TeamStructureReferences : TeamOffStructureInstantiateHandler<T>
        {
            
        }
    }
}
