using System;
using System.Collections.Generic;
using Characters;
using CombatEffects;
using UnityEngine;

namespace Passives
{
    [Serializable]
    public class PassivesHolder
    {
        [SerializeField] 
        private List<SInjectionPassiveBase> openingPassives;
        /// <summary>
        /// Passives that only counts in the opening phase
        /// </summary>
        public List<SInjectionPassiveBase> OpeningPassives => openingPassives;

        [SerializeField]
        private List<SCombatActionPassivePreset> actionPassives;
        public List<SCombatActionPassivePreset> ActionPassives => actionPassives;

        [SerializeField] 
        private List<SCombatReactionPassivePreset> reactionPassives;
        public List<SCombatReactionPassivePreset> ReactionPassives => reactionPassives;

        public PassivesHolder()
        {
            openingPassives = new List<SInjectionPassiveBase>();
            actionPassives = new List<SCombatActionPassivePreset>();
            reactionPassives = new List<SCombatReactionPassivePreset>();
        }

        public PassivesHolder(PassivesHolder copyFrom)
        {
            //Opening are not altered during combat since it just once
            openingPassives = copyFrom.OpeningPassives; 
            actionPassives = new List<SCombatActionPassivePreset>(copyFrom.ActionPassives);
            reactionPassives = new List<SCombatReactionPassivePreset>(copyFrom.reactionPassives);
        }

        public void DoOpeningPassives(CombatingEntity user)
        {
            foreach (SInjectionPassiveBase passive in openingPassives)
            {
                passive.InjectPassive(user);
            }
        }

    }

}
