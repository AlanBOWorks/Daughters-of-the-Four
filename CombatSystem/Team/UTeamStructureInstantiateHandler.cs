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
    public abstract class UTeamStructureInstantiateHandler<T> : MonoBehaviour,
        IOppositionTeamStructureRead<ITeamFullRoleStructureRead<T>>,
        ICombatStatesListener,

        IMainElementInstantiationListener<T>
        where T : MonoBehaviour
    {
        [Title("References - Player")]
        [SerializeField,HorizontalGroup()] private TeamStructureReferences playerTeamType;
        [Title("References - Enemy")]
        [SerializeField,HorizontalGroup()] private TeamStructureReferences enemyTeamType;

        [Title("Params")] 
        [SerializeField,DisableInPlayMode] private bool hidePrefabs = true;
        [SerializeField] private EnumTeam.StructureType structureType;

        [ShowInInspector,HideInEditorMode]
        private Dictionary<CombatEntity, T> _activeElementsDictionary;
        public IReadOnlyDictionary<CombatEntity, T> ActiveElementsDictionary => _activeElementsDictionary;

        private void Awake()
        {
            _activeElementsDictionary = new Dictionary<CombatEntity, T>(EnumTeam.OppositeTeamRolesAmount);
            
            InstantiateElements();
            HidePrefabs();


            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
        }

        private void InstantiateElements()
        {
            playerTeamType.InstantiateElements();
            enemyTeamType.InstantiateElements();
        }

        private void HidePrefabs()
        {
            if(!hidePrefabs) return;
            playerTeamType.HidePrefab();
            enemyTeamType.HidePrefab();
        }


        public ITeamFullRoleStructureRead<T> PlayerTeamType => playerTeamType;
        public ITeamFullRoleStructureRead<T> EnemyTeamType => enemyTeamType;
        

        public virtual void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _activeElementsDictionary.Clear();
            var callListeners = GetComponents<IMainElementInstantiationListener<T>>();
            foreach (var listener in callListeners)
            {
                listener.OnCombatPreStarts();
            }


            IterationTeam(in playerTeam, in playerTeamType);
            IterationTeam(in enemyTeam, in enemyTeamType);

            foreach (var listener in callListeners)
            {
                listener.OnFinishPreStarts();
            }

            void IterationTeam(in CombatTeam team, in TeamStructureReferences references)
            {
                var mainMembers = GetStructureMembers(in team);
                int notNullIndex = 0;
                for (var i = 0; i < mainMembers.Count; i++)
                {
                    var element = references.Members[i];
                    var member = mainMembers[i];

                    foreach (var listener in callListeners)
                    {
                        listener.OnIterationCall(in element, in member, notNullIndex);
                    }

                    if (member == null) continue;

                    _activeElementsDictionary.Add(member, element);
                    notNullIndex++;
                }
            }
        }

        public void OnCombatPreStarts()
        {
            
        }

        public void OnFinishPreStarts()
        {
            
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


        public abstract void OnIterationCall(in T element, in CombatEntity entity, int notNullIndex);

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

    public interface IMainElementInstantiationListener<T>
    {
        void OnCombatPreStarts();
        void OnFinishPreStarts();
        void OnIterationCall([NotNull] in T element, [CanBeNull] in CombatEntity entity, int notNullIndex);
    }
}
