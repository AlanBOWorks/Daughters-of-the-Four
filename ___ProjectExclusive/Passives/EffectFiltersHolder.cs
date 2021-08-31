using System.Collections.Generic;
using Characters;
using CombatEffects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Passives
{
    public class EffectFiltersHolder
    {
        private readonly CombatingEntity _user;
        /// <summary>
        /// Filters for doing a <see cref="Skills.CombatSkill"/>
        /// </summary>
        [ShowInInspector]
        private List<SEffectFilterPassive> _actionFilters;

        /// <summary>
        /// Filters when receiving a <see cref="Skills.CombatSkill"/>
        /// </summary>
        [ShowInInspector]
        private List<SEffectFilterPassive> _reactionFilters;


        public EffectFiltersHolder(CombatingEntity user)
        {
            _user = user;
        }

        public void Clear()
        {
            _actionFilters.Clear();
            _reactionFilters.Clear();
        }

        public void AddFilter(SEffectFilterPassive passive, bool isActiveType)
        {
            if(isActiveType)
                AddFilter(ref _actionFilters,passive);
            else
                AddFilter(ref _reactionFilters,passive);
        }

        private void AddFilter(ref List<SEffectFilterPassive> filters, SEffectFilterPassive passive)
        {
            if (filters == null)
            {
                filters = new List<SEffectFilterPassive> {passive};
                return;
            }
            filters.Add(passive);
        }

        private static void DoFiltering(List<SEffectFilterPassive> filters,
            IEffectBase effectCheck, CombatingEntity target,  ref float effectAddition)
        {
            if(filters == null) return;
            
            foreach (SEffectFilterPassive filter in filters)
            {
                filter.DoVariation(effectCheck, target,ref effectAddition);
            }
        }

        public void DoFilterOnAction(IEffectBase effectCheck, ref float effectValue)
            => DoFiltering(_actionFilters, effectCheck, _user, ref effectValue);

        public void DoFilterOnReaction(IEffectBase effectCheck, ref float effectValue)
            => DoFiltering(_reactionFilters, effectCheck, _user, ref effectValue);

    }
}
