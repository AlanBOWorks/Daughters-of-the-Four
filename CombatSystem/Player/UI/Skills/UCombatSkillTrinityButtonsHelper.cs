using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UCombatSkillTrinityButtonsHelper : MonoBehaviour,
        ICombatStatesListener, ITempoTeamStatesListener,
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
            
        }

        public void OnOffEntityRequestSequence(CombatEntity entity, bool canAct)
        {
        }

        public void OnTrinityEntityFinishSequence(CombatEntity entity)
        {
            if (_activeMembers.Count > 0)
            {
                SwitchToFirstMember();
                return;
            }
            holder.DisableHolder();

            void SwitchToFirstMember()
            {
                var firstActive = _activeMembers[0];
                holder.SwitchControllingEntity(in firstActive);
            }
        }

        public void OnOffEntityFinishSequence(CombatEntity entity)
        {
        }

        private IReadOnlyList<CombatEntity> _activeMembers;
        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _activeMembers = playerTeam.GetTrinityActiveMembers();
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

        public void OnTempoStartControl(in CombatTeamControllerBase controller,in CombatEntity firstEntity)
        {
            holder.SwitchControllingEntity(in firstEntity);
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
        }

    }
}
