using System.Collections.Generic;
using CombatEffects;
using CombatEntity;
using CombatSkills;
using CombatTeam;

namespace CombatSystem.Events
{
    public sealed class SystemEventsHolder : CharacterEventsHolder<SkillValuesHolders,CombatingEntity,EffectResolution>,
        ITeamStateChangeListener<CombatingTeam>
    {
        public SystemEventsHolder() : base()
        {
            _teamStateChangeListeners = new List<ITeamStateChangeListener<CombatingTeam>>();
        }

        private readonly List<ITeamStateChangeListener<CombatingTeam>> _teamStateChangeListeners;

        public void Subscribe(ITeamStateChangeListener<CombatingTeam> listener) =>
            _teamStateChangeListeners.Add(listener);

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
    }
}
