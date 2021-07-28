using System;
using System.Collections.Generic;
using Characters;
using Skills;
using UnityEngine;

namespace Passives
{
    public class CombatPassivesHolder : PassivesHolder, IPassivesFiltersHolder
    {
        public readonly CombatingEntity User;

        public CombatPassivesHolder(CombatingEntity user, 
            PassivesHolder copyFrom, IPassivesFiltersHolder sharedPassives) 
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
        private List<SBuffSkillPreset> passives;

        public OpeningPassives(int size = 0)
        {
            passives = new List<SBuffSkillPreset>(size);
        }

        public void AddPassive(SBuffSkillPreset passive)
        {
            if (passives.Contains(passive)) return;
            passives.Add(passive);
        }

        public void RemovePassive(SBuffSkillPreset passive)
        {
            passives.Remove(passive);
        }

        public void DoOpeningPassives(CombatingEntity user)
        {
            if (passives.Count <= 0) return;
            foreach (SBuffSkillPreset buffEffect in passives)
            {
                buffEffect.DoEffects(user);
            }
        }

    }


    public interface IPassivesFiltersHolder
    {
        List<SActionPassiveFilterPreset> ActionFilterPassives { get; }
        List<SReactionPassiveFilterPreset> ReactionFilterPassives { get; }
        PassivesFilters GetFilters();
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
