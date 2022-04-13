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
    public class UCombatWaitSkill : MonoBehaviour, ITempoEntityStatesListener, ITeamEventListener
    {
        [Title("Dependencies")]
        [SerializeField] 
        private UCombatSkillButtonsHolder holder;
        [SerializeField] 
        private UCombatSkillButton skillButton;

        [Title("Data")]
        [ShowInInspector,HideInEditorMode]
        private WaitCombatSkill _waitCombatSkill;
        private void Awake()
        {
            _waitCombatSkill = new WaitCombatSkill();
            skillButton.Injection(_waitCombatSkill);
            skillButton.Injection(holder);

            PlayerCombatSingleton.PlayerCombatEvents.DiscriminationEventsHolder.Subscribe(this);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.DiscriminationEventsHolder.UnSubscribe(this);
        }

        private void AddToHoldersDictionary()
        {
            holder.AddToDictionary(_waitCombatSkill, in skillButton);
        }

        public void OnEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            AddToHoldersDictionary();
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
            //todo hide
            _waitCombatSkill.ResetCost();
            skillButton.HideButton();
            
        }

        public void OnEntityFinishSequence(CombatEntity entity)
        {
        }

        public void OnEntityWaitSequence(CombatEntity entity)
        {
        }

        public void OnStanceChange(in CombatTeam team, in EnumTeam.StanceFull switchedStance)
        {
            // Problem: holder's Dictionary clears itself on this stanceChange event
            AddToHoldersDictionary();
        }

        public void OnControlChange(in CombatTeam team, in float phasedControl, in bool isBurst)
        {
        }
    }
}
