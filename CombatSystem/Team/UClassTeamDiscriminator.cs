using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using UnityEngine;

namespace CombatSystem.Team
{
    /// <summary>
    /// Holds Player's and Enemy's objects references and discriminates based on [<see cref="CombatTeam"/>]
    /// </summary>
    public abstract class UClassTeamDiscriminator<T> : MonoBehaviour, ICombatPreparationListener where T : new()
    {
        [SerializeField]
        private T playerType = new T();
        [SerializeField]
        private T enemyType = new T();

        private void Awake()
        {
            CombatSystemSingleton.EventsHolder.Subscribe(this);
        }

        public T GetElement(in CombatTeam team)
        {
            bool isPlayer = UtilsTeam.IsPlayerTeam(in team);

            return isPlayer 
                ? playerType 
                : enemyType;
        }

        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            OnCombatPrepares(in playerType,in playerTeam);
            OnCombatPrepares(in enemyType, in enemyTeam);
        }

        protected abstract void OnCombatPrepares(in T element, in CombatTeam team);
    }

    /// <summary>
    /// Discriminates based on [<see cref="ITeamEventListener"/>]'s callbacks
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UClassTeamDiscriminatorListener<T> : UClassTeamDiscriminator<T>, ITeamEventListener where T : new()
    {
        public void OnStanceChange(in CombatTeam team, in EnumTeam.StanceFull switchedStance)
        {
            var element = GetElement(in team);
            OnStanceChange(in element,in switchedStance);

        }

        protected abstract void OnStanceChange(in T element, in EnumTeam.StanceFull switchStance);

        public void OnControlChange(in CombatTeam team, in float phasedControl, in bool isBurst)
        {
            var element = GetElement(in team);
            OnControlChange(in element, in phasedControl, in isBurst);
        }

        protected abstract void OnControlChange(in T element, in float phasedControl, in bool isBurst);
    }


    public abstract class UTeamDiscriminator<T> : MonoBehaviour, ICombatPreparationListener
    {
        [SerializeField]
        private T playerType;
        [SerializeField]
        private T enemyType;

        private void Awake()
        {
            CombatSystemSingleton.EventsHolder.Subscribe(this);
        }

        public T GetElement(in CombatTeam team)
        {
            bool isPlayer = UtilsTeam.IsPlayerTeam(in team);

            return isPlayer
                ? playerType
                : enemyType;
        }

        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            OnCombatPrepares(in playerType, in playerTeam);
            OnCombatPrepares(in enemyType, in enemyTeam);
        }

        protected abstract void OnCombatPrepares(in T element, in CombatTeam team);
    }

    public abstract class UTeamDiscriminatorListener<T> : UTeamDiscriminator<T>, ITeamEventListener
    {
        public void OnStanceChange(in CombatTeam team, in EnumTeam.StanceFull switchedStance)
        {
            var element = GetElement(in team);
            OnStanceChange(in element, in switchedStance);

        }

        protected abstract void OnStanceChange(in T element, in EnumTeam.StanceFull switchStance);

        public void OnControlChange(in CombatTeam team, in float phasedControl, in bool isBurst)
        {
            var element = GetElement(in team);
            OnControlChange(in element, in phasedControl, in isBurst);
        }

        protected abstract void OnControlChange(in T element, in float phasedControl, in bool isBurst);
    }
}
