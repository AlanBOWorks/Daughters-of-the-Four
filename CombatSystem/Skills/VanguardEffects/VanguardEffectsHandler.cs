using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills.Effects;
using CombatSystem.Stats;
using CombatSystem.Team;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills.VanguardEffects
{
    public sealed class VanguardEffectsHandler : ISkillUsageListener
    {
        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
            _callVanguardEffects = false;
        }


        private bool _callVanguardEffects;
        private SkillUsageValues _skillUsageValues;
        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
            var skill = values.UsedSkill;
            if (!UtilsSkill.IsOffensiveSkill(skill)) return;

            var attacker = values.Performer;
            var onTarget = values.Target;
            var targetTeam = onTarget.Team;

            if (targetTeam.Contains(attacker)) return;


            var counterEffectsHolder = onTarget.CounterEffects;
            var punishEffectsHolder = targetTeam.PunishEffectsHolder;

            if (!counterEffectsHolder.HasEffects() && !punishEffectsHolder.HasEffects()) return;

            _callVanguardEffects = true;
            _skillUsageValues = values;

        }
        public void OnCombatSkillFinish(CombatEntity performer)
        {
            if(!_callVanguardEffects) return;


            var attacker = _skillUsageValues.Performer;
            var onTarget = _skillUsageValues.Target;
            var targetTeam = onTarget.Team;

            var counterEffectsHolder = onTarget.CounterEffects;
            var punishEffectsHolder = targetTeam.PunishEffectsHolder;

            CombatSystemSingleton.EventsHolder.OnVanguardEffectsPerform(attacker, onTarget);
            counterEffectsHolder.OnOffensiveDone(onTarget, attacker);
            punishEffectsHolder.OnOffensiveDone(onTarget, attacker);
        }
    }


    public abstract class VanguardEffectsHolder
    {
        protected VanguardEffectsHolder(CombatEntity mainResponsibleEntity)
        {
            if (mainResponsibleEntity == null) return;

            MainEntity = mainResponsibleEntity;
            VanguardEffects = new VanguardSkillAccumulationWrapper();

        }

        [Title("Entity")]
        [ShowInInspector]
        public readonly CombatEntity MainEntity;

        [Title("Effects")]
        [ShowInInspector]
        protected readonly VanguardSkillAccumulationWrapper VanguardEffects;
        public CombatEntity GetMainEntity() => MainEntity;





        public bool CanPerform()
        {
            var stats = MainEntity.Stats;
            return UtilsCombatStats.IsInitiativeEnough(stats)
                   && UtilsCombatStats.IsAlive(stats)
                   && HasEffects();
        }

        public bool HasEffects() => VanguardEffects.HasEffects();

        public void Clear()
        {
            VanguardEffects.Clear();
        }

        public void AddSkillEffects(IVanguardSkill skill)
        {
            var punishEffects = GetVanguardEffects(skill);
            foreach (var effect in punishEffects)
            {
                VanguardEffects.AddEffect(effect.TargetType, effect.Effect, effect.EffectValue);
            }
        }

        protected abstract IEnumerable<PerformEffectValues> GetVanguardEffects(IVanguardSkill skill);

        public virtual void OnOffensiveDone(CombatEntity onTarget, CombatEntity attacker)
        {
            PerformEffects(onTarget, attacker);
        }

        protected void PerformEffects(CombatEntity effectPerformer, CombatEntity attacker)
        {
            foreach ((EnumsEffect.TargetType targetType, var dictionary) in VanguardEffects.GetEnumerableWithType())
            {
                DoEffect(targetType, dictionary);
            }

            void DoEffect(EnumsEffect.TargetType type, Dictionary<IEffect, float> dictionary)
            {
                foreach ((IEffect effect, var value) in dictionary)
                {
                    var vanguardEffectValues = new VanguardEffectUsageValues(
                        effectPerformer, attacker,
                        effect, value);
                    UtilsCombatSkill.DoVanguardEffects(type, in vanguardEffectValues);
                }
            }
        }

    }

    public class VanguardSkillAccumulationWrapper : SkillTargetingStructureClass<Dictionary<IEffect, float>>
    {
        private bool _hasEffects;
        public bool HasEffects() => _hasEffects;

        public void AddEffect(EnumsEffect.TargetType targetType, IEffect effect, float accumulation)
        {
            _hasEffects = true;
            var collection = UtilsEffect.GetElement(targetType, this);
            if (collection.ContainsKey(effect))
            {
                collection[effect] += accumulation;
            }
            else
            {
                collection.Add(effect, accumulation);
            }
        }

        public void Clear()
        {
            var enumerable = UtilsEffect.GetEnumerable(this);
            foreach (var dictionary in enumerable)
            {
                dictionary.Clear();
            }

            _hasEffects = false;
        }

        public VanguardSkillAccumulationWrapper() : base(true)
        {
        }
    }
}
