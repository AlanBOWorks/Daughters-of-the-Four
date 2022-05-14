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

        [ShowInInspector]
        private IEnumerable<CombatEntity> _interactionMembers;

        public IEnumerable<CombatEntity> SingleType => _singleTarget;
        public IEnumerable<CombatEntity> TargetLine => _aliveLine;
        public IEnumerable<CombatEntity> TargetTeam => _aliveTeam;
        public IEnumerable<CombatEntity> GetSkillInteractions() => _interactionMembers;

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

        public void HandleInteractions(in CombatSkill skill)
        {
            var effects = skill.GetEffects();
            EnumsEffect.TargetType interactionType = EnumsEffect.TargetType.Target;
            foreach (var effect in effects)
            {
                var effectType = effect.TargetType;
                switch (effectType)
                {
                    case EnumsEffect.TargetType.PerformerTeam:
                    case EnumsEffect.TargetType.TargetTeam:
                    case EnumsEffect.TargetType.All:
                        _interactionMembers = _aliveTeam;
                        return;
                    case EnumsEffect.TargetType.PerformerLine:
                    case EnumsEffect.TargetType.TargetLine:
                        interactionType = EnumsEffect.TargetType.TargetLine;
                        break;
                }
            }

            if (interactionType == EnumsEffect.TargetType.TargetLine)
                _interactionMembers = _aliveLine;
            else
                _interactionMembers = _singleTarget;

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

        public IEnumerable<CombatEntity> GetInteractions() =>
            PerformerHelper.GetSkillInteractions().Concat(TargetHelper.GetSkillInteractions());

        public void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            PerformerHelper.Clear();
            TargetHelper.Clear();

            bool isAlly = performer.Team.Contains(target);
            PerformerHelper.HandleAlive(in performer, in isAlly);
            TargetHelper.HandleAlive(in target,in isAlly);

            PerformerHelper.HandleInteractions(in usedSkill);
            TargetHelper.HandleInteractions(in usedSkill);
        }

        public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
        }

        public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
        {
        }

        public void OnSkillFinish(in CombatEntity performer)
        {
            
        }
    }

}
