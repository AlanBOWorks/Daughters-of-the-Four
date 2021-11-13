using System;
using CombatEntity;
using CombatSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatTeam
{
    public abstract class UPersistentGroupStructureBase<T> : MonoBehaviour, ICombatGroupsStructureRead<T>,
        ICombatPreparationListener, ICombatDisruptionListener
        where T : UnityEngine.Object
    {
        [SerializeField,HorizontalGroup()] 
        protected TeamElement playerTeam = new TeamElement();
        [SerializeField, HorizontalGroup()] 
        protected TeamElement enemyTeam = new TeamElement();

        public ITeamRoleStructureRead<T> GetPlayerTeam() => playerTeam;
        public ITeamRoleStructureRead<T> GetEnemyTeam() => enemyTeam;

        protected virtual void Awake()
        {
            var preparationHandler = CombatSystemSingleton.CombatPreparationHandler;
            preparationHandler.Subscribe(this as ICombatPreparationListener);
            preparationHandler.Subscribe(this as ICombatDisruptionListener);
        }

        public void OnPreparationCombat(CombatingTeam preparedPlayerTeam, CombatingTeam preparedEnemyTeam)
        {
            UtilsTeam.DoActionOnTeam(preparedPlayerTeam, playerTeam, DoInjection);
            UtilsTeam.DoActionOnTeam(preparedEnemyTeam, enemyTeam, DoInjection);
        }

        protected abstract void DoInjection(CombatingEntity entity, T element);
        public abstract void OnAfterLoadsCombat();
        public abstract void OnCombatPause();
        public abstract void OnCombatResume();
        public abstract void OnCombatExit();

        [Serializable]
        protected class TeamElement : ITeamRoleStructureRead<T>
        {
            [SerializeField]
            private T vanguard;
            [SerializeField]
            private T attacker;
            [SerializeField]
            private T support;


            public T Vanguard => vanguard;
            public T Attacker => attacker;
            public T Support => support;
        }

    }
}
