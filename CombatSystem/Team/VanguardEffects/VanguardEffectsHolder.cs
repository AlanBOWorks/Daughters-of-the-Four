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
        public VanguardEffectsHolder(CombatTeam team)
        {
            var teamRoles = team.GetRolesStructures();
            _mainEntities = UtilsTeam.GetFrontMostElement(teamRoles);

            if (_mainEntities == null) return;

            _effectDictionaries = new VanguardEffectDictionaries<IVanguardSkill, PerformEffectValues>();
            _offensiveRecordsDictionaries = new VanguardEffectDictionariesBasic<CombatEntity, int>();
        }

        private readonly IReadOnlyList<CombatEntity> _mainEntities;


        private readonly VanguardEffectDictionaries<IVanguardSkill, PerformEffectValues> _effectDictionaries;
        private readonly VanguardEffectDictionariesBasic<CombatEntity, int> _offensiveRecordsDictionaries;


        public void AddEffect(
            IVanguardSkill skill, 
            EnumsVanguardEffects.VanguardEffectType effectType,
            PerformEffectValues effectValue)
        {
            if (_mainEntities == null) return;
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
            if(_mainEntities == null || enemyPerformer.Team.Contains(in onTarget)) return;

            bool isMainEntitiesTarget = _mainEntities.Contains(onTarget);
            if (isMainEntitiesTarget)
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
