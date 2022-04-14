using System;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.UI;
using CombatSystem.Skills;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.Skills
{
    public class UCombatWaitSkill : MonoBehaviour
    {
        [Title("Dependencies")]
        [SerializeField] 
        private UCombatSkillButtonsHolder holder;
        [SerializeField] 
        private UCombatSkillButton skillButton;


        [Title("Data")]
        [ShowInInspector,HideInEditorMode]
        private WaitCombatSkill _waitCombatSkill;

        public UCombatSkillButton GetButton() => skillButton;
        public WaitCombatSkill GetSkill() => _waitCombatSkill;

        private void Awake()
        {
            _waitCombatSkill = new WaitCombatSkill();
            gameObject.SetActive(false);
            InjectIntoButton();
        }

        public void InjectIntoButton()
        {
            skillButton.Injection(_waitCombatSkill);
            skillButton.Injection(holder);
        }

        public void ResetCost()
        {
            _waitCombatSkill.ResetCost();
        }
    }
}
