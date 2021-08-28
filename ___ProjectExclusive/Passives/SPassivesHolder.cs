using System;
using System.Collections.Generic;
using ___ProjectExclusive;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Passives
{

    [CreateAssetMenu(fileName = "Passive HOLDER - N [Passives]", 
        menuName = "Combat/Passives/Holder", order = -100)]
    public class SPassivesHolder : ScriptableObject, IPassiveHolder
    {
        [SerializeField] 
        private string passiveName = "NULL";

        [SerializeField,DisableInEditorMode,DisableInPlayMode] 
        private List<SPassiveInjectorBase> passives = new List<SPassiveInjectorBase>();


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
            name = $"Passive HOLDER - {passiveName} [Passives]";
            UtilsGame.UpdateAssetName(this);
        } 
#endif
    }

    [Serializable]
    public class PassiveHolder : IPassiveInjector
    {
        [SerializeField] 
        private List<SPassiveInjectorBase> passives = new List<SPassiveInjectorBase>();
        public void InjectPassive(CombatingEntity entity)
        {
            for (int i = 0; i < passives.Count; i++)
            {
                passives[i].InjectPassive(entity);
            }
        }
    }
}
