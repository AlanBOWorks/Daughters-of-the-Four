using System;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UCombatSkillTrinityButtonsHelper : MonoBehaviour,
        ICombatStatesListener,
        ITempoDedicatedEntityStatesListener
    {
        [SerializeField] private UCombatSkillButtonsHolder holder;

        private void Awake()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.Subscribe(this);
        }


        public void OnTrinityEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            Debug.Log("TRIN");
            if(!canAct) return;

            holder.SwitchControllingEntity(in entity);
        }

        public void OnOffEntityRequestSequence(CombatEntity entity, bool canAct)
        {
        }

        public void OnTrinityEntityFinishSequence(CombatEntity entity)
        {

        }

        public void OnOffEntityFinishSequence(CombatEntity entity)
        {
        }

        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
        }

        public void OnCombatStart()
        {
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
        {
        }
    }
}
