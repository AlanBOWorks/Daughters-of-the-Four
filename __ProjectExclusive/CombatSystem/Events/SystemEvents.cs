using System;
using System.Collections.Generic;
using CombatEffects;
using CombatEntity;
using CombatSkills;
using CombatTeam;
using Sirenix.OdinInspector;

namespace CombatSystem.Events
{
    public class SystemEventsHolder :
        CombatEventsHolderBase<ISkillParameters, CombatingEntity,CombatingSkill,SkillComponentResolution>,
        ICombatSystemEvents
    {
        public SystemEventsHolder() : base()
        {
            _teamStateChangeListeners = new HashSet<ITeamStateChangeListener<CombatingTeam>>();
            _skillEvents = new HashSet<ISkillEventListener>();
            _animationEvents = new HashSet<IAnimationsListener<SkillValuesHolders>>();
        }

        [Title("Team")]
        [ShowInInspector]
        private readonly HashSet<ITeamStateChangeListener<CombatingTeam>> _teamStateChangeListeners;

        [HorizontalGroup("On Action events",
            Title = "________________________ On Actions Events ________________________")]
        [ShowInInspector]
        private readonly HashSet<ISkillEventListener> _skillEvents;
        [HorizontalGroup("On Action events")]
        [ShowInInspector]
        private readonly HashSet<IAnimationsListener<SkillValuesHolders>> _animationEvents;

        public void Subscribe(SystemEventsHolder listener)
        {
            base.Subscribe(listener);

            _teamStateChangeListeners.Add(listener);
            _skillEvents.Add(listener);
            _animationEvents.Add(listener);
        }
        public void Subscribe(ICombatSystemEvents listener)
        {
            Subscribe(listener 
                as IOffensiveActionReceiverListener<ISkillParameters, CombatingSkill, SkillComponentResolution>);
            Subscribe(listener 
                as ISupportActionReceiverListener<ISkillParameters, CombatingSkill, SkillComponentResolution>);
            Subscribe(listener 
                as IVitalityChangeListener<ISkillParameters, CombatingSkill>);
            Subscribe(listener 
                as ITempoListener<CombatingEntity>);
            Subscribe(listener 
                as ITempoAlternateListener<CombatingEntity>);
            Subscribe(listener 
                as IRoundListener<CombatingEntity>);

            _teamStateChangeListeners.Add(listener);
            _skillEvents.Add(listener);
            _animationEvents.Add(listener);
        }

        public void Subscribe(ITeamStateChangeListener<CombatingTeam> listener) => 
            _teamStateChangeListeners.Add(listener);
        public void Subscribe(ISkillEventListener listener) => 
            _skillEvents.Add(listener);
        public void Subscribe(IAnimationsListener<SkillValuesHolders> listener) => 
            _animationEvents.Add(listener);

        public void OnStanceChange(CombatingTeam holder, EnumTeam.TeamStance switchStance)
        {
            foreach (var listener in _teamStateChangeListeners)
            {
                listener.OnStanceChange(holder,switchStance);
            }
        }

        public void OnMemberDeath(CombatingTeam holder, CombatingEntity member)
        {
            foreach (var listener in _teamStateChangeListeners)
            {
                listener.OnMemberDeath(holder, member);
            }
        }

        public void OnSkillUse(SkillValuesHolders values)
        {
            foreach (var listener in _skillEvents)
            {
                listener.OnSkillUse(values);
            }
        }

        public void OnSkillCostIncreases(SkillValuesHolders values)
        {
            foreach (var listener in _skillEvents)
            {
                listener.OnSkillCostIncreases(values);
            }
        }

        public void OnBeforeAnimation(SkillValuesHolders element)
        {
            foreach (var listener in _animationEvents)
            {
                listener.OnBeforeAnimation(element);
            }
        }

        public void OnAnimationClimax(SkillValuesHolders element)
        {
            foreach (var listener in _animationEvents)
            {
                listener.OnAnimationClimax(element);
            }
        }

        public void OnAnimationHaltFinish(SkillValuesHolders element)
        {
            foreach (var listener in _animationEvents)
            {
                listener.OnAnimationHaltFinish(element);
            }
        }
    }
}
