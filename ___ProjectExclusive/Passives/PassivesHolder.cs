using System;
using System.Collections.Generic;
using Characters;
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

        public PassivesHolder()
        {
            openingPassives = new List<SInjectionPassiveBase>();
        }

        public PassivesHolder(PassivesHolder copyFrom)
        {
            //Opening are not altered during combat since it just once
            openingPassives = copyFrom.OpeningPassives; 
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
