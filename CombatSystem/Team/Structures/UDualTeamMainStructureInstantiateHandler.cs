using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public abstract class UDualTeamMainStructureInstantiateHandler<T> : UDualTeamStructureInstantiateHandlerBase<T>,
        IOppositionTeamStructureRead<ITeamFlexStructureRead<T>>
      
        where T : MonoBehaviour
    {
        [Title("References - Player")]
        [SerializeField,HorizontalGroup()] private TeamStructureReferences playerTeamType;
        [Title("References - Enemy")]
        [SerializeField,HorizontalGroup()] private TeamStructureReferences enemyTeamType;

        [Title("Params")] 
        [SerializeField,DisableInPlayMode] private bool hidePrefabs = true;
        [SerializeField] private EnumTeam.StructureType structureType;

        

        protected override void InstantiateElements()
        {
            playerTeamType.InstantiateElements();
            enemyTeamType.InstantiateElements();
        }

        protected override void HidePrefabs()
        {
            if(!hidePrefabs) return;
            playerTeamType.HidePrefab();
            enemyTeamType.HidePrefab();
        }

        protected TeamMainStructureInstantiateHandler<T> GetPlayerHandler() => playerTeamType;
        protected TeamMainStructureInstantiateHandler<T> GetEnemyHandler() => enemyTeamType;

        public ITeamFlexStructureRead<T> PlayerTeamType => playerTeamType;
        public ITeamFlexStructureRead<T> EnemyTeamType => enemyTeamType;
        
        protected override void IterationTeam(in CombatTeam team, bool isPlayerElement, in IEntityElementInstantiationListener<T>[] callListeners)
        {
            var mainMembers = GetStructureMembers(in team);
            IterationValues.ResetState(isPlayerElement);
            var references = (isPlayerElement) ? playerTeamType : enemyTeamType;

            for (var i = 0; i < mainMembers.Count; i++)
            {
                var element = references.Members[i];
                var member = mainMembers[i];

                


                foreach (var listener in callListeners)
                {
                    listener.OnIterationCall(in element, in member, in IterationValues);
                }

                if (member == null)
                {
                    IterationValues.IncrementAsNull();
                    continue;
                }

                ActiveElementsDictionary.Add(member, element);


                IterationValues.IncrementAsNotNull();
            }

            references.activeCount = IterationValues.NotNullIndex;
        }
      

        private IReadOnlyList<CombatEntity> GetStructureMembers(in CombatTeam team)
        {
            switch (structureType)
            {
                case EnumTeam.StructureType.TeamPosition:
                    return team.MainPositioningMembers;
                default:
                    return team.MainRoleMembers;
            }
        }

        [Serializable]
        private sealed class TeamStructureReferences : TeamMainStructureInstantiateHandler<T>
        {
            
        }
    }


}
