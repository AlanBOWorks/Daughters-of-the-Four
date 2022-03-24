using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using UnityEngine;

namespace CombatSystem.Team
{
    public abstract class UTeamDiscriminator<T> : MonoBehaviour, ICombatPreparationListener where T : new()
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

    public abstract class UTeamDiscriminatorListener<T> : UTeamDiscriminator<T>, ITeamEventListener where T : new()
    {
        public void OnStanceChange(in CombatTeam team, in EnumTeam.Stance switchedStance)
        {
            var element = GetElement(in team);
            OnStanceChange(in element,in switchedStance);

        }

        protected abstract void OnStanceChange(in T element, in EnumTeam.Stance switchStance);

        public void OnControlChange(in CombatTeam team, in float phasedControl, in bool isBurst)
        {
            var element = GetElement(in team);
            OnControlChange(in element, in phasedControl, in isBurst);
        }

        protected abstract void OnControlChange(in T element, in float phasedControl, in bool isBurst);
    }

    public abstract class UTeamMonoDiscriminator<T> : MonoBehaviour, ICombatPreparationListener where T : MonoBehaviour
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

    public abstract class UTeamMonoDiscriminatorListener<T> : UTeamMonoDiscriminator<T>, ITeamEventListener
        where T : MonoBehaviour
    {
        public void OnStanceChange(in CombatTeam team, in EnumTeam.Stance switchedStance)
        {
            var element = GetElement(in team);
            OnStanceChange(in element, in switchedStance);

        }

        protected abstract void OnStanceChange(in T element, in EnumTeam.Stance switchStance);

        public void OnControlChange(in CombatTeam team, in float phasedControl, in bool isBurst)
        {
            var element = GetElement(in team);
            OnControlChange(in element, in phasedControl, in isBurst);
        }

        protected abstract void OnControlChange(in T element, in float phasedControl, in bool isBurst);
    }
}
