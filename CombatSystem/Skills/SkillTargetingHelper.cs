using System;
using System.Collections.Generic;
using System.Linq;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills.Effects;
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
            Clear();

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

    public class SkillTargetingHandler
    {
        public SkillTargetingHandler()
        {
            PerformerHelper = new SkillTargetingHelper();
            TargetHelper = new SkillTargetingHelper();
            InteractionsEntities = new HashSet<CombatEntity>();
        }
        [Title("Helpers")]
        [ShowInInspector,HorizontalGroup()] 
        protected readonly SkillTargetingHelper PerformerHelper;
        [ShowInInspector,HorizontalGroup()] 
        protected readonly SkillTargetingHelper TargetHelper;

        [Title("Interactions")]
        [ShowInInspector]
        protected readonly HashSet<CombatEntity> InteractionsEntities;

        public ISkillInteractionStructureRead<IEnumerable<CombatEntity>> PerformerType => PerformerHelper;
        public ISkillInteractionStructureRead<IEnumerable<CombatEntity>> TargetType => TargetHelper;
        public IEnumerable<CombatEntity> AllType => PerformerType.TargetTeam.Concat(TargetType.TargetTeam);

        public IEnumerable<CombatEntity> GetInteractions() => InteractionsEntities;

        public void HandleSkill(CombatEntity performer, CombatSkill usedSkill, CombatEntity target)
        {

            bool isAlly = performer.Team.Contains(target);
            TargetHelper.HandleAlive(in target, in isAlly);
            PerformerHelper.HandleAlive(in performer, in isAlly);

            HandleInteractions(usedSkill, target);
        }


        private void HandleInteractions(CombatSkill usedSkill, CombatEntity target)
        {
            InteractionsEntities.Clear();

            AddInteractionEntity(in target);
            var effects = usedSkill.GetEffects();
            IEnumerable<CombatEntity> performerGroup = null;
            IEnumerable<CombatEntity> targetGroup = null;
            HandleEffects(effects, ref performerGroup, ref targetGroup,
                out bool setPerformer, out bool setTarget);

            if (performerGroup == null && setPerformer) performerGroup = PerformerHelper.SingleType;
            if (targetGroup == null && setTarget) targetGroup = TargetHelper.SingleType;

            HandleInteractionsGroup(in performerGroup);
            HandleInteractionsGroup(in targetGroup);
        }

        private void HandleEffects(IEnumerable<PerformEffectValues> effects, 
            ref IEnumerable<CombatEntity> performerGroup, 
            ref IEnumerable<CombatEntity> targetGroup,
            out bool performerSingleTarget,
            out bool targetSingleTarget)
        {
            performerSingleTarget = false;
            targetSingleTarget = false;
            foreach (var effect in effects)
            {
                var effectType = effect.TargetType;
                switch (effectType)
                {
                    case EnumsEffect.TargetType.Target:
                        targetSingleTarget = true;
                        break;
                    case EnumsEffect.TargetType.Performer:
                        performerSingleTarget = true;
                        break;

                    case EnumsEffect.TargetType.TargetLine:
                        if (targetGroup == null)
                            targetGroup = TargetHelper.TargetLine;
                        break;
                    case EnumsEffect.TargetType.TargetTeam:
                        targetGroup = TargetHelper.TargetTeam;
                        break;

                    case EnumsEffect.TargetType.PerformerLine:
                        if (performerGroup == null)
                            performerGroup = PerformerHelper.TargetLine;
                        break;
                    case EnumsEffect.TargetType.PerformerTeam:
                        performerGroup = PerformerHelper.TargetTeam;
                        break;

                    case EnumsEffect.TargetType.All:
                        performerGroup = PerformerType.TargetTeam;
                        targetGroup = TargetHelper.TargetTeam;
                        return;
                }

            }
        }

        private void HandleInteractionsGroup(in IEnumerable<CombatEntity> group)
        {
            if(group == null) return;
            foreach (var entity in group)
            {
                AddInteractionEntity(in entity);
            }
        }

        protected virtual void AddInteractionEntity(in CombatEntity entity)
        {
            if (InteractionsEntities.Contains(entity)) return;
            InteractionsEntities.Add(entity);
        }
    }

}
