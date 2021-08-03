using System;
using System.Collections.Generic;
using _CombatSystem;
using _Team;
using Characters;
using CombatEffects;
using Skills;
using UnityEngine;

namespace Passives
{
    public class CombatPassivesHolder : PassivesHolder, IPassivesFiltersHolder, IPassivesFilterHandler
    {
        public readonly CombatingEntity User;
        private readonly HarmonyBuffInvoker _harmonyPassive;

        public CombatPassivesHolder(CombatingEntity user, 
            PassivesHolder copyFrom, IPassivesFiltersHolder sharedPassives,
            HarmonyBuffInvoker harmonyBuffInvoker) 
            : base(copyFrom)
        {
            User = user;

            foreach (var actionPassive in sharedPassives.ActionFilterPassives)
            {
                AttackingSkills.ActionFilterPassives.Add(actionPassive);
                NeutralSkills.ActionFilterPassives.Add(actionPassive);
                DefendingSkills.ActionFilterPassives.Add(actionPassive);
            }
            foreach (var reactionPassive in sharedPassives.ReactionFilterPassives)
            {
                AttackingSkills.ReactionFilterPassives.Add(reactionPassive);
                NeutralSkills.ReactionFilterPassives.Add(reactionPassive);
                DefendingSkills.ReactionFilterPassives.Add(reactionPassive);
            }

            _harmonyPassive = harmonyBuffInvoker;
        }

        private ITeamCombatControlStats GetTeamControl()
        {
            return User.CharacterGroup.Team;
        }

        public List<SActionPassiveFilterPreset> ActionFilterPassives
        {
            get
            {
                var passives = UtilsSkill.GetElement(this, User);
                return passives.ActionFilterPassives;
            }
        }
        public List<SReactionPassiveFilterPreset> ReactionFilterPassives
        {
            get
            {
                var passives = UtilsSkill.GetElement(this, User);
                return passives.ReactionFilterPassives;
            }
        }

        public PassivesFilters GetFilters()
        {
            var holder = UtilsSkill.GetElement(this, User);
            return holder.GetFilters();
        }


        public void DoActionPassiveFilter
            (ref EffectArguments arguments, ref float currentValue, float originalValue)
        {
            var passives = ActionFilterPassives;
            foreach (SActionPassiveFilterPreset passive in passives)
            {
                passive.DoPassiveFilter(
                    ref arguments,
                    ref currentValue,
                    originalValue);
            }

            _harmonyPassive?.DoActionPassiveFilter(
                ref arguments,
                ref currentValue,
                originalValue);

            var teamPassives 
                = GetTeamControl().GetCurrentPassives().ActionFilterPassives;
            foreach (SActionPassiveFilterPreset passive in teamPassives)
            {
                passive.DoPassiveFilter(
                    ref arguments,
                    ref currentValue,
                    originalValue);
            }
        }

        public void DoReActionPassiveFilter
            (ref EffectArguments arguments, ref float currentValue, float originalValue)
        {
            var passives = ReactionFilterPassives;
            foreach (SReactionPassiveFilterPreset passive in passives)
            {
                passive.DoPassiveFilter(
                    ref arguments,
                    ref currentValue,
                    originalValue);
            }

            _harmonyPassive?.DoReActionPassiveFilter(
                ref arguments,
                ref currentValue,
                originalValue);

            var teamPassives
                = GetTeamControl().GetCurrentPassives().ReactionFilterPassives;
            foreach (SReactionPassiveFilterPreset passive in teamPassives)
            {
                passive.DoPassiveFilter(
                    ref arguments,
                    ref currentValue,
                    originalValue);
            }
        }
    }

    [Serializable]
    public class PassivesHolder : ISkillPositions<IPassivesFiltersHolder>
    {
        [SerializeField] protected FilterPassivesHolder attackingFilterPassives;
        [SerializeField] protected FilterPassivesHolder neutralFilterPassives;
        [SerializeField] protected FilterPassivesHolder defendingFilterPassives;

        public PassivesHolder()
        {
            attackingFilterPassives = new FilterPassivesHolder();
            neutralFilterPassives = new FilterPassivesHolder();
            defendingFilterPassives = new FilterPassivesHolder();
        }

        public PassivesHolder(ISkillPositions<IPassivesFiltersHolder> copyFrom)
        {
            //Opening are not altered during combat since it just once
            attackingFilterPassives = new FilterPassivesHolder(copyFrom.AttackingSkills);
            neutralFilterPassives = new FilterPassivesHolder(copyFrom.NeutralSkills);
            defendingFilterPassives = new FilterPassivesHolder(copyFrom.DefendingSkills);
        }
        

        public IPassivesFiltersHolder AttackingSkills => attackingFilterPassives;
        public IPassivesFiltersHolder NeutralSkills => neutralFilterPassives;
        public IPassivesFiltersHolder DefendingSkills => defendingFilterPassives;


    }

    [Serializable]
    public class FilterPassivesHolder : IPassivesFiltersHolder
    {
        public static FilterPassivesHolder EmptyHolder = new FilterPassivesHolder();

        [SerializeField]
        private List<SActionPassiveFilterPreset> actionFilterPassives;
        public List<SActionPassiveFilterPreset> ActionFilterPassives => actionFilterPassives;

        [SerializeField]
        private List<SReactionPassiveFilterPreset> reactionFilterPassives;
        public List<SReactionPassiveFilterPreset> ReactionFilterPassives => reactionFilterPassives;
        public PassivesFilters GetFilters()
        {
            return new PassivesFilters(actionFilterPassives,reactionFilterPassives);
        }

        public FilterPassivesHolder()
        {
            actionFilterPassives = new List<SActionPassiveFilterPreset>();
            reactionFilterPassives = new List<SReactionPassiveFilterPreset>();
        }

        public FilterPassivesHolder(IPassivesFiltersHolder copyFrom)
        {
            //Opening are not altered during combat since it just once
            actionFilterPassives = new List<SActionPassiveFilterPreset>(copyFrom.ActionFilterPassives);
            reactionFilterPassives = new List<SReactionPassiveFilterPreset>(copyFrom.ReactionFilterPassives);
        }
    }


    [Serializable]
    public class OpeningPassives
    {
        [SerializeField]
        private List<SOpeningPassivesPreset> passives;

        [SerializeField] 
        private List<SAuraPassive> auras;

        public OpeningPassives(int size = 0)
        {
            passives = new List<SOpeningPassivesPreset>(size);
            auras = new List<SAuraPassive>(size);
        }

        public void AddPassive(SOpeningPassivesPreset passive)
        {
            if (passives.Contains(passive)) return;
            passives.Add(passive);
        }

        public void RemovePassive(SOpeningPassivesPreset passive)
        {
            passives.Remove(passive);
        }

        public void DoOpeningPassives(CombatingEntity user)
        {
            if (passives.Count > 0)
                foreach (SOpeningPassivesPreset buffEffect in passives)
                {
                    buffEffect.DoDirectEffects(user, user);
                }
            if(auras.Count > 0)
                foreach (SAuraPassive aura in auras)
                {
                    user.CharacterGroup.Team.InjectAura(aura);
                }
        }

    }


    public interface IPassivesFiltersHolder
    {
        List<SActionPassiveFilterPreset> ActionFilterPassives { get; }
        List<SReactionPassiveFilterPreset> ReactionFilterPassives { get; }
        PassivesFilters GetFilters();
    }

    public interface IPassivesFilterHandler
    {
        void DoActionPassiveFilter
            (ref EffectArguments arguments, ref float currentValue, float originalValue);

        void DoReActionPassiveFilter
            (ref EffectArguments arguments, ref float currentValue, float originalValue);
    }

    public struct PassivesFilters : IPassivesFiltersHolder
    {
        public PassivesFilters(List<SActionPassiveFilterPreset> actionFilterPassives, List<SReactionPassiveFilterPreset> reactionFilterPassives)
        {
            ActionFilterPassives = actionFilterPassives;
            ReactionFilterPassives = reactionFilterPassives;
        }

        public readonly List<SActionPassiveFilterPreset> ActionFilterPassives { get; }
        public readonly List<SReactionPassiveFilterPreset> ReactionFilterPassives { get; }
        public PassivesFilters GetFilters() => this;
    }
}
