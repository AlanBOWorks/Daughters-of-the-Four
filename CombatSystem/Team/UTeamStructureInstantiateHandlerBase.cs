using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public abstract class UTeamStructureInstantiateHandlerBase<T> : MonoBehaviour,
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
            ActiveElementsDictionary = new Dictionary<CombatEntity, T>(EnumTeam.OppositeTeamRolesAmount);
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

        public virtual void OnCombatFinish()
        { }

        public virtual void OnCombatQuit()
        { }
    }
}
