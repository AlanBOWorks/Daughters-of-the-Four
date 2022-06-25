using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team.VanguardEffects
{
    public sealed class VanguardEffectsHolder 
    {
        public VanguardEffectsHolder(CombatEntity mainResponsibleEntity)
        {
            if (mainResponsibleEntity == null) return;

            _mainEntity = mainResponsibleEntity;
            _effectDictionaries = new VanguardEffectDictionaries<IVanguardSkill, int>();
            _offensiveRecordsDictionaries = new VanguardEffectDictionaries<CombatEntity, int>();
        }


        [Title("Entity")]
        [ShowInInspector]
        private readonly CombatEntity _mainEntity;

        [Title("Effects")]
        [ShowInInspector]
        private readonly VanguardEffectDictionaries<IVanguardSkill, int> _effectDictionaries;
        [ShowInInspector]
        private readonly VanguardEffectDictionaries<CombatEntity, int> _offensiveRecordsDictionaries;


        public CombatEntity GetMainEntity() => _mainEntity;

        public IVanguardEffectStructureRead<Dictionary<IVanguardSkill, int>> GetEffectsStructure() =>
            _effectDictionaries;

        public IVanguardEffectStructureRead<Dictionary<CombatEntity, int>> GetOffensiveRecordsStructure() =>
            _offensiveRecordsDictionaries;




        // Revenge effects are added in both wrappers, but if there wasn't any record of offensive
        // actions, then the effects can't be invoked.
        public bool HasRevengeEffects() => _offensiveRecordsDictionaries.VanguardRevengeType.Count > 0;
        // Same as [Revenge];
        public bool HasPunishEffects() => _offensiveRecordsDictionaries.VanguardPunishType.Count > 0;

        public bool IsMainEntityTurn()
        {
            var stats = _mainEntity.Stats;
            return UtilsCombatStats.IsInitiativeEnough(stats);
        }
        public bool CanPerform()
        {
            var stats = _mainEntity.Stats;
            return UtilsCombatStats.IsInitiativeEnough(stats)
                   && UtilsCombatStats.IsAlive(stats)
                   && HasEffects();
        }

        public bool HasEffects() => HasRevengeEffects()|| HasPunishEffects();

        public void Clear()
        {
            _effectDictionaries.VanguardRevengeType.Clear();
            _effectDictionaries.VanguardPunishType.Clear();

            _offensiveRecordsDictionaries.VanguardRevengeType.Clear();
            _offensiveRecordsDictionaries.VanguardPunishType.Clear();
        }

        public void AddEffect(IVanguardSkill skill) 
            => AddEffect(skill, skill.GetVanguardEffectType());
        public void AddEffect(
            IVanguardSkill skill, 
            EnumsVanguardEffects.VanguardEffectType effectType)
        {
            if (_mainEntity == null) return;
            if (skill == null) return;

            int accumulatedAmount;
            var targetCollection = 
                UtilsVanguardEffects.GetElement(effectType, _effectDictionaries);
            if (targetCollection.ContainsKey(skill))
            {
                accumulatedAmount = targetCollection[skill]++;
            }
            else
            {
                accumulatedAmount = 1;
                targetCollection.Add(skill, accumulatedAmount);
            }

            VanguardSkillAccumulation eventValues = new VanguardSkillAccumulation(
                skill, effectType, accumulatedAmount);
            CombatSystemSingleton.EventsHolder.OnVanguardEffectSubscribe(in eventValues);
        }

        public void OnOffensiveDone(CombatEntity enemyPerformer, CombatEntity onTarget)
        {
            if(_mainEntity == null || enemyPerformer.Team.Contains(onTarget)) return;

            if (onTarget.PositioningType == _mainEntity.PositioningType)
            {
                // Problem: if there's no revenge Skill in the Collection, then accumulating
                // revenge counts won't make sense > should accumulates if there's at least
                // one or more Skill
                var revengeDictionary = _effectDictionaries.VanguardRevengeType;
                if(revengeDictionary.Count == 0) return;
                HandleDictionary(_offensiveRecordsDictionaries.VanguardRevengeType);
                HandleEvents(EnumsVanguardEffects.VanguardEffectType.Revenge);
            }
            else
            {
                // Same as above
                var punishDictionary = _effectDictionaries.VanguardPunishType;
                if(punishDictionary.Count == 0) return;
                HandleDictionary(_offensiveRecordsDictionaries.VanguardPunishType);
                HandleEvents(EnumsVanguardEffects.VanguardEffectType.Punish);
            }


            void HandleDictionary(Dictionary<CombatEntity, int> dictionary)
            {
                if (dictionary.ContainsKey(enemyPerformer))
                {
                    dictionary[enemyPerformer]++;
                    return;
                }
                dictionary.Add(enemyPerformer,1);
            }

            void HandleEvents(EnumsVanguardEffects.VanguardEffectType type)
            {
                var eventsHolder = CombatSystemSingleton.EventsHolder;
                eventsHolder.OnVanguardEffectIncrement(type,enemyPerformer);
            }
        }





        private class VanguardEffectDictionaries<TKey, TValue> : 
            IVanguardEffectStructureRead<Dictionary<TKey, TValue>>
        {
            public VanguardEffectDictionaries()
            {
                VanguardRevengeType = new Dictionary<TKey, TValue>();
                VanguardPunishType = new Dictionary<TKey, TValue>();
            }
            [ShowInInspector,HorizontalGroup()]
            public Dictionary<TKey, TValue> VanguardRevengeType { get; }
            [ShowInInspector,HorizontalGroup()]
            public Dictionary<TKey, TValue> VanguardPunishType { get; }
        }
    }

    public readonly struct VanguardSkillAccumulation
    {
        public readonly IVanguardSkill Skill;
        public readonly EnumsVanguardEffects.VanguardEffectType Type;
        public readonly int AccumulatedAmount;

        public VanguardSkillAccumulation(
            IVanguardSkill skill, 
            EnumsVanguardEffects.VanguardEffectType type, 
            int accumulatedAmount)
        {
            Skill = skill;
            Type = type;
            AccumulatedAmount = accumulatedAmount;
        }

        public IEnumerable<PerformEffectValues> GeneratePerformValues()
        {
            foreach (PerformEffectValues effect in Skill.GetPerformVanguardEffects())
            {
                yield return new PerformEffectValues
                    (effect.Effect,
                    effect.EffectValue * AccumulatedAmount,
                    effect.TargetType);
            }
        }
    }
}
