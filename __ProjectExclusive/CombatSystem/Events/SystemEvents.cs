using System.Collections.Generic;
using CombatEffects;
using CombatEntity;
using CombatSkills;
using CombatTeam;
using Sirenix.OdinInspector;

namespace CombatSystem.Events
{
    public sealed class SystemEventsHolder : CharacterEventsHolder<SkillValuesHolders,CombatingEntity,SkillComponentResolution>,
        ITeamStateChangeListener<CombatingTeam>,
        ISkillEventListener
    {
        public SystemEventsHolder() : base()
        {
            _teamStateChangeListeners = new List<ITeamStateChangeListener<CombatingTeam>>();
            _skillEvents = new SkillEvents();
        }

        [Title("Team")]
        [ShowInInspector]
        private readonly List<ITeamStateChangeListener<CombatingTeam>> _teamStateChangeListeners;
        [Title("Skills")]
        [ShowInInspector]
        private readonly SkillEvents _skillEvents;

        public void Subscribe(SystemEventsHolder eventsHolder)
        {
            base.Subscribe(eventsHolder);
            Subscribe(eventsHolder as ISkillEventListener);
            Subscribe(eventsHolder as ITeamStateChangeListener<CombatingTeam>);
        }

        public void Subscribe(ITeamStateChangeListener<CombatingTeam> listener) 
            => _teamStateChangeListeners.Add(listener);
        public void Subscribe(ISkillEventListener listener)
            => _skillEvents.Subscribe(listener);

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
            => _skillEvents.OnSkillUse(values);
    }
}
