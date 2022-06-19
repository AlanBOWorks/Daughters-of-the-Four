using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
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
            _offensiveRecordsDictionaries = new VanguardEffectDictionariesBasic<CombatEntity, int>();
        }


        [Title("Entity")]
        [ShowInInspector]
        private readonly CombatEntity _mainEntity;

        [Title("Effects")]
        [ShowInInspector]
        private readonly VanguardEffectDictionaries<IVanguardSkill, int> _effectDictionaries;
        [ShowInInspector]
        private readonly VanguardEffectDictionariesBasic<CombatEntity, int> _offensiveRecordsDictionaries;


        public void AddEffect(
            IVanguardSkill skill, 
            EnumsVanguardEffects.VanguardEffectType effectType)
        {
            if (_mainEntity == null) return;
            if (skill == null) return;


            var targetCollection = 
                UtilsVanguardEffects.GetElement(effectType, _effectDictionaries);
            if (targetCollection.ContainsKey(skill))
            {
                targetCollection[skill]++;
                return;
            }
            targetCollection.Add(skill, 1);
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




        private sealed class VanguardEffectDictionaries<TKey, TValue> : VanguardEffectDictionariesBasic<TKey, TValue>, 
            IVanguardEffectsStructuresRead<Dictionary<TKey,TValue>>
        {
            public VanguardEffectDictionaries() : base()
            {
                VanguardDelayImproveType = new Dictionary<TKey, TValue>();
            }
            [ShowInInspector]
            public Dictionary<TKey, TValue> VanguardDelayImproveType { get; }
        }

        private class VanguardEffectDictionariesBasic<TKey, TValue> : 
            IVanguardEffectStructureBaseRead<Dictionary<TKey, TValue>>
        {
            public VanguardEffectDictionariesBasic()
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
}
