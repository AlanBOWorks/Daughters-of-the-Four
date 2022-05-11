using System.Collections.Generic;
using System.Linq;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

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
            ICollection<CombatEntity> aliveLine = _aliveLine;
            UtilsTargetsCollection.HandleLine(in aliveLine,in target, in isAlly);
        }

        private void HandleTeam(in CombatEntity target)
        {
            ICollection<CombatEntity> aliveTeam = _aliveTeam;
            UtilsTargetsCollection.HandleTeam(in aliveTeam, in target);
        }

        public void Clear()
        {
            _singleTarget[0] = null;
            _aliveLine.Clear();
            _aliveTeam.Clear();
        }
    }

    public class SkillTargetingHandler : ISkillUsageListener
    {
        public SkillTargetingHandler()
        {
            PerformerHelper = new SkillTargetingHelper();
            TargetHelper = new SkillTargetingHelper();
            ;
        }
        [ShowInInspector,HorizontalGroup()] 
        protected readonly SkillTargetingHelper PerformerHelper;
        [ShowInInspector,HorizontalGroup()] 
        protected readonly SkillTargetingHelper TargetHelper;

        public ISkillInteractionStructureRead<IEnumerable<CombatEntity>> PerformerType => PerformerHelper;
        public ISkillInteractionStructureRead<IEnumerable<CombatEntity>> TargetType => TargetHelper;
        public IEnumerable<CombatEntity> AllType => PerformerType.TargetTeam.Concat(TargetType.TargetTeam);


        public void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            bool isAlly = performer.Team.Contains(target);
            PerformerHelper.HandleAlive(in performer, in isAlly);
            TargetHelper.HandleAlive(in target,in isAlly);
        }

        public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
        }

        public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
        {
        }

        public void OnSkillFinish(in CombatEntity performer)
        {
            PerformerHelper.Clear();
            TargetHelper.Clear();
        }
    }
}
