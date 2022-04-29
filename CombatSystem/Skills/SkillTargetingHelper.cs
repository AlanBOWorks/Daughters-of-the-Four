using System.Collections.Generic;
using System.Linq;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;

namespace CombatSystem.Skills
{
    public sealed class SkillTargetingHelper : ISkillInteractionStructureRead<IEnumerable<CombatEntity>>
    {
        public SkillTargetingHelper()
        {
            _singleTarget = new CombatEntity[1];
            _aliveLine = new HashSet<CombatEntity>();
            _aliveTeam = new HashSet<CombatEntity>();
        }

        [ShowInInspector]
        private readonly CombatEntity[] _singleTarget;
        [ShowInInspector]
        private readonly HashSet<CombatEntity> _aliveLine;
        [ShowInInspector]
        private readonly HashSet<CombatEntity> _aliveTeam;

        public IEnumerable<CombatEntity> SingleType => _singleTarget;
        public IEnumerable<CombatEntity> TargetLine => _aliveLine;
        public IEnumerable<CombatEntity> TargetTeam => _aliveTeam;

        public void HandleAlive(in CombatEntity entity,in bool isAlly)
        {
            HandleSingleTarget(in entity);
            HandleLine(in entity,in isAlly);
            HandleTeam(in entity);
        }

        private void HandleSingleTarget(in CombatEntity target)
        {
            _singleTarget[0] = target;
        }

        private void HandleLine(in CombatEntity target, in bool isAlly)
        {
            _aliveLine.Clear(); //safe clear

            var targetLine = isAlly 
                ? UtilsTarget.GetSupportLine(target) 
                : UtilsTarget.GetOffensiveLine(target);
            foreach (var member in targetLine)
            {
                if (UtilsTarget.CanBeTargeted(in member)) 
                    _aliveLine.Add(member);
            }
        }

        private void HandleTeam(in CombatEntity target)
        {
            _aliveTeam.Clear(); //safe clear

            foreach (var member in target.Team)
            {
                if (UtilsTarget.CanBeTargeted(in member))
                    _aliveTeam.Add(member);
            }
        }

        public void Clear()
        {
            _singleTarget[0] = null;
            _aliveLine.Clear();
            _aliveTeam.Clear();
        }
    }

    public sealed class SkillTargetingHandler : ISkillUsageListener
    {
        public SkillTargetingHandler()
        {
            _performerHelper = new SkillTargetingHelper();
            _targetHelper = new SkillTargetingHelper();
            ;
        }
        [ShowInInspector,HorizontalGroup()]
        private readonly SkillTargetingHelper _performerHelper;
        [ShowInInspector,HorizontalGroup()]
        private readonly SkillTargetingHelper _targetHelper;

        public ISkillInteractionStructureRead<IEnumerable<CombatEntity>> PerformerType => _performerHelper;
        public ISkillInteractionStructureRead<IEnumerable<CombatEntity>> TargetType => _targetHelper;
        public IEnumerable<CombatEntity> AllType => PerformerType.TargetTeam.Concat(TargetType.TargetTeam);


        public void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            bool isAlly = performer.Team.Contains(target);
            _performerHelper.HandleAlive(in performer, in isAlly);
            _targetHelper.HandleAlive(in target,in isAlly);
        }

        public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
        }

        public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
        {
        }

        public void OnSkillFinish()
        {
            _performerHelper.Clear();
            _targetHelper.Clear();
        }
    }
}
