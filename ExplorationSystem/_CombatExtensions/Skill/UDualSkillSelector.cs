using System;
using CombatSystem.Skills;
using CombatSystem.Team;
using ExplorationSystem.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ExplorationSystem
{
    public class UDualSkillSelector : MonoBehaviour
    {
        [SerializeField] private UDualSkillCraftHandler mainHandler;
        [SerializeField] private UDualSkillCombinationElement myHolder;

        [SerializeField] private bool isMainSkill;

        private void Awake()
        {
            if(!isMainSkill)
                myHolder.HideExtraData();
        }

        public void SelectSkill(IFullSkill skill, EnumTeam.Stance stance)
        {
            if (skill == null)
            {
                myHolder.ClearAndPrint();
                return;
            }

            myHolder.DoInjection(skill);
            mainHandler.InjectSkill(skill, stance, isMainSkill);
        }

        [Button,DisableInEditorMode]
        private void TestInjection(SSkillPresetBase skill, EnumTeam.Stance stance) 
            => SelectSkill(skill, stance);
    }
}
