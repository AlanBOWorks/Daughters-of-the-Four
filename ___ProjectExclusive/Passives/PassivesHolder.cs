using System;
using System.Collections.Generic;
using Characters;
using CombatEffects;
using UnityEngine;

namespace Passives
{
    [Serializable]
    public class PassivesHolder : PassivesHolderBase
    {
        [SerializeField] 
        private List<SEffectBuffBase> openingPassives;
        /// <summary>
        /// Passives that only counts in the opening phase
        /// </summary>
        public List<SEffectBuffBase> OpeningPassives => openingPassives;

       
        public PassivesHolder() : base()
        {
            openingPassives = new List<SEffectBuffBase>();
        }

        public PassivesHolder(PassivesHolder copyFrom) : base(copyFrom)
        {
            //Opening are not altered during combat since it just once
            openingPassives = copyFrom.OpeningPassives; 
        }

        public void DoOpeningPassives(CombatingEntity user) 
            => DoOpeningPassives(user, openingPassives);

    }

    [Serializable]
    public class PassivesHolderBase
    {

        [SerializeField]
        private List<SActionPassiveFilterPreset> actionFilterPassives;
        public List<SActionPassiveFilterPreset> ActionFilterPassives => actionFilterPassives;

        [SerializeField]
        private List<SReactionPassiveFilterPreset> reactionFilterPassives;
        public List<SReactionPassiveFilterPreset> ReactionFilterPassives => reactionFilterPassives;

        public PassivesHolderBase()
        {
            actionFilterPassives = new List<SActionPassiveFilterPreset>();
            reactionFilterPassives = new List<SReactionPassiveFilterPreset>();
        }

        public PassivesHolderBase(PassivesHolder copyFrom)
        {
            //Opening are not altered during combat since it just once
            actionFilterPassives = new List<SActionPassiveFilterPreset>(copyFrom.ActionFilterPassives);
            reactionFilterPassives = new List<SReactionPassiveFilterPreset>(copyFrom.reactionFilterPassives);
        }

        public PassivesHolderBase(
            PassivesHolder copyFrom, CombatingEntity user, List<SEffectBuffBase> openingPassives)
        : this(copyFrom)
        {
            DoOpeningPassives(user,openingPassives);
        }


        public void DoOpeningPassives(CombatingEntity user, List<SEffectBuffBase> openingPassives)
        {
            if(openingPassives == null || openingPassives.Count <= 0) return;

            foreach (SEffectBuffBase buffEffect in openingPassives)
            {
                buffEffect.DoEffect(user,user);
            }
        }
    }

}
