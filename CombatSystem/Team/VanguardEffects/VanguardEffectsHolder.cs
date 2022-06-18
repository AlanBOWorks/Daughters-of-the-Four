using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using UnityEngine;

namespace CombatSystem.Team.VanguardEffects
{
    public sealed class VanguardEffectsHolder 
    {
        public VanguardEffectsHolder(CombatEntity mainResponsibleEntity)
        {
            if (mainResponsibleEntity == null) return;

            _mainEntity = mainResponsibleEntity;
            _effectDictionaries = new VanguardEffectDictionaries<IVanguardSkill, PerformEffectValues>();
            _offensiveRecordsDictionaries = new VanguardEffectDictionariesBasic<CombatEntity, int>();
        }



        private readonly CombatEntity _mainEntity;

        private readonly VanguardEffectDictionaries<IVanguardSkill, PerformEffectValues> _effectDictionaries;
        private readonly VanguardEffectDictionariesBasic<CombatEntity, int> _offensiveRecordsDictionaries;


        public void AddEffect(
            IVanguardSkill skill, 
            EnumsVanguardEffects.VanguardEffectType effectType,
            PerformEffectValues effectValue)
        {
            if (_mainEntity == null) return;
            if (skill == null) return;

            var targetCollection = UtilsVanguardEffects.GetElement(effectType, _effectDictionaries);
            if (targetCollection.ContainsKey(skill))
            {
                targetCollection[skill]++;
                return;
            }
            targetCollection.Add(skill, effectValue);
        }

        public void OnOffensiveDone(CombatEntity enemyPerformer, CombatEntity onTarget)
        {
            if(_mainEntity == null || enemyPerformer.Team.Contains(in onTarget)) return;

            if (onTarget.PositioningType == _mainEntity.PositioningType)
            {
                var revengeDictionary = _effectDictionaries.VanguardRevengeType;
                if(revengeDictionary.Count == 0) return;
                HandleDictionary(_offensiveRecordsDictionaries.VanguardRevengeType);
            }
            else
            {
                var punishDictionary = _effectDictionaries.VanguardPunishType;
                if(punishDictionary.Count == 0) return;
                HandleDictionary(_offensiveRecordsDictionaries.VanguardPunishType);
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
        }




        private sealed class VanguardEffectDictionaries<TKey, TValue> : VanguardEffectDictionariesBasic<TKey, TValue>, 
            IVanguardEffectsStructuresRead<Dictionary<TKey,TValue>>
        {
            public VanguardEffectDictionaries() : base()
            {
                VanguardDelayImproveType = new Dictionary<TKey, TValue>();
            }
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
            public Dictionary<TKey, TValue> VanguardRevengeType { get; }
            public Dictionary<TKey, TValue> VanguardPunishType { get; }
        }


    }
}