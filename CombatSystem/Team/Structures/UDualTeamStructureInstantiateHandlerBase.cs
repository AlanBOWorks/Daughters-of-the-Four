using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public abstract class UDualTeamStructureInstantiateHandlerBase<T> : MonoBehaviour,
        ICombatStatesListener,

        IEntityElementInstantiationListener<T>
        where T : MonoBehaviour
    {
        [ShowInInspector, HideInEditorMode] 
        protected Dictionary<CombatEntity, T> ActiveElementsDictionary;
        public IReadOnlyDictionary<CombatEntity, T> GetActiveElementsDictionary() => ActiveElementsDictionary;
        protected TeamStructureIterationValues IterationValues;

        private void Awake()
        {
            ActiveElementsDictionary = new Dictionary<CombatEntity, T>();
            IterationValues = new TeamStructureIterationValues();

            InstantiateElements();
            HidePrefabs();


            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
        }

        protected abstract void InstantiateElements();
        protected abstract void HidePrefabs();

        public virtual void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            ActiveElementsDictionary.Clear();
            var callListeners = GetComponents<IEntityElementInstantiationListener<T>>();
            foreach (var listener in callListeners)
            {
                listener.OnCombatPreStarts();
            }


            IterationTeam(in playerTeam, true, in callListeners);
            IterationTeam(in enemyTeam, false, in callListeners);

            foreach (var listener in callListeners)
            {
                listener.OnFinishPreStarts();
            }
        }

        protected abstract void IterationTeam(in CombatTeam team, bool isPlayerElement, in IEntityElementInstantiationListener<T>[] callListeners);


        public virtual void OnCombatPreStarts()
        { }

        public virtual void OnFinishPreStarts()
        { }

        public abstract void OnIterationCall(in T element, in CombatEntity entity, in TeamStructureIterationValues values);

        public virtual void OnCombatStart()
        { }

        public void OnCombatEnd()
        { }

        public virtual void OnCombatFinish(bool isPlayerWin)
        { }

        public virtual void OnCombatQuit()
        { }
    }

    public abstract class UDualTeamStructureInstantiateHandlerBaseSerializable<T> : UDualTeamStructureInstantiateHandlerBase<T>
        where T : MonoBehaviour
    {
        [Title("References - Player")]
        [SerializeField, HorizontalGroup()] private TeamStructureReferences playerTeamType;
        [Title("References - Enemy")]
        [SerializeField, HorizontalGroup()] private TeamStructureReferences enemyTeamType;

        [Title("Params")]
        [SerializeField, DisableInPlayMode] private bool hidePrefabs = true;
        [SerializeField] private EnumTeam.StructureType structureType;



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

        protected TeamBasicStructureInstantiateHandler<T> GetPlayerHandler() => playerTeamType;
        protected TeamBasicStructureInstantiateHandler<T> GetEnemyHandler() => enemyTeamType;

        public TeamBasicGroupStructure<T> PlayerTeamType => playerTeamType;
        public TeamBasicGroupStructure<T> EnemyTeamType => enemyTeamType;

        protected override void IterationTeam(in CombatTeam team, bool isPlayerElement, in IEntityElementInstantiationListener<T>[] callListeners)
        {
            var mainMembers = GetStructureMembers(in team);
            IterationValues.ResetState(isPlayerElement);
            var references = (isPlayerElement) ? playerTeamType : enemyTeamType;
            var elements = references.Members;

            int notFlexRoleThreshold = elements.Length;
            for (var i = 0; i < notFlexRoleThreshold; i++)
            {
                var element = elements[i];
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
        private sealed class TeamStructureReferences : TeamBasicStructureInstantiateHandler<T>
        {
            
        }
    }
}
