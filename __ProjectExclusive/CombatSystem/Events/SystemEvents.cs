using System.Collections.Generic;
using CombatEffects;
using CombatEntity;
using CombatSkills;
using CombatTeam;
using Sirenix.OdinInspector;

namespace CombatSystem.Events
{
    public class SystemEventsHolder :
        CharacterEventsHolder<CombatEntityPairAction, CombatingEntity,CombatingSkill,SkillComponentResolution>,
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

        /*public override void SubscribeListener(object listener)
        {
            base.SubscribeListener(listener);
            if(listener is ISkillEventListener skillEventListener)
                Subscribe(skillEventListener);
            if (listener is ITeamStateChangeListener<CombatingTeam> teamChangeListener)
                Subscribe(teamChangeListener);
        }*/

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
