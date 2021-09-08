using System;
using System.Collections.Generic;
using ___ProjectExclusive;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Passives
{

    [CreateAssetMenu(fileName = "SET Passives - N [PreSet]", 
        menuName = "Combat/Passives/Passives SET", order = -100)]
    public class SPassivesSet : ScriptableObject, IPassiveHolder
    {
        [SerializeField] 
        private string passiveName = "NULL";

        [SerializeField,DisableInEditorMode,DisableInPlayMode] 
        private List<SPassiveInjector> passives = new List<SPassiveInjector>();


        public void InjectPassive(CombatingEntity entity)
        {
            for (int i = 0; i < passives.Count; i++)
            {
                passives[i].InjectPassive(entity);
            }
        }


#if UNITY_EDITOR
       

        [Button(ButtonSizes.Large)]
        private void UpdateAssetName()
        {
            name = $"SET Passives - {passiveName} [PreSeT]";
            UtilsGame.UpdateAssetName(this);
        } 
#endif
    }

    [Serializable]
    public class PassiveHolder : IPassiveInjector
    {
        [SerializeField] 
        private List<SPassiveInjector> passives = new List<SPassiveInjector>();
        public void InjectPassive(CombatingEntity entity)
        {
            for (int i = 0; i < passives.Count; i++)
            {
                passives[i].InjectPassive(entity);
            }
        }
    }
}