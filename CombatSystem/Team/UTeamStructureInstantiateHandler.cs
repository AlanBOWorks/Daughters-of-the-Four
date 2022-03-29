using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public abstract class UTeamStructureInstantiateHandler<T> : MonoBehaviour,
        IOppositionTeamStructureRead<ITeamFullRoleStructureRead<T>>,
        ICombatStatesListener
        
        where T : MonoBehaviour
    {
        [SerializeField,HorizontalGroup()] private TeamStructureReferences playerTeamType;
        [SerializeField,HorizontalGroup()] private TeamStructureReferences enemyTeamType;
        [SerializeField] private EnumTeam.StructureType structureType;

        private Dictionary<CombatEntity, T> _activeElementsDictionary;
        public IReadOnlyDictionary<CombatEntity, T> ActiveElementsDictionary => _activeElementsDictionary;

        private void Awake()
        {
            playerTeamType.InstantiateElements();
            enemyTeamType.InstantiateElements();

            _activeElementsDictionary = new Dictionary<CombatEntity, T>(EnumTeam.OppositeTeamRolesAmount);

            CombatSystemSingleton.EventsHolder.Subscribe(this);
        }

        public ITeamFullRoleStructureRead<T> PlayerTeamType => playerTeamType;
        public ITeamFullRoleStructureRead<T> EnemyTeamType => enemyTeamType;
        

        public virtual void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _activeElementsDictionary.Clear();

            IterationTeam(in playerTeam, in playerTeamType);
            IterationTeam(in enemyTeam, in enemyTeamType);
        }

        public virtual void OnCombatStart()
        {
        }

        public virtual void OnCombatFinish()
        {
        }

        public virtual void OnCombatQuit()
        {
        }


        private void IterationTeam(in CombatTeam team, in TeamStructureReferences references)
        {
            var mainMembers = GetStructureMembers(in team);
            int notNullIndex = 0;
            for (var i = 0; i < mainMembers.Count; i++)
            {
                var element = references.Members[i];
                var member = mainMembers[i];

                OnIterationCall(in element, in member, notNullIndex);

                if (member == null) continue;

                _activeElementsDictionary.Add(member, element);
                notNullIndex++;
            }
        }

        protected abstract void OnIterationCall([NotNull] in T element, [CanBeNull] in CombatEntity entity, int notNullIndex);

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
        private sealed class TeamStructureReferences : TeamStructureInstantiateHandler<T>
        {
            
        }

    }
}
