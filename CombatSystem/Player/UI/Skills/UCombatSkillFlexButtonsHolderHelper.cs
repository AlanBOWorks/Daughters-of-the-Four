using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UCombatSkillFlexButtonsHolderHelper : MonoBehaviour, 
        ICombatStatesListener,
        ITempoDedicatedEntityStatesListener, 
        ITempoTeamStatesListener
    {
        [SerializeField] private UCombatSkillButtonsHolder flexButtonsHolder;

        private void Awake()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.DiscriminationEventsHolder.ManualSubscribe(this as ITempoDedicatedEntityStatesListener);
            playerEvents.DiscriminationEventsHolder.ManualSubscribe(this as ITempoTeamStatesListener);
            playerEvents.ManualSubscribe(this as ICombatStatesListener);
        }

        private bool _flexIsActive;
        [ShowInInspector]
        private CombatEntity _flexEntity;
        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _flexEntity = playerTeam.FlexType;
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

        public void OnTrinityEntityRequestSequence(CombatEntity entity, bool canAct)
        {
           
        }

        public void OnOffEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            _flexIsActive = canAct;
        }

        public void OnTrinityEntityFinishSequence(CombatEntity entity)
        {
        }

        public void OnOffEntityFinishSequence(CombatEntity entity)
        {
            if (entity != _flexEntity) return;

            _flexIsActive = false;
            flexButtonsHolder.HideAll();
        }


        public void OnTempoStartControl(in CombatTeamControllerBase controller)
        {
            if (_flexIsActive)
            {
                flexButtonsHolder.SwitchControllingEntity(in _flexEntity);
            }
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            flexButtonsHolder.HideAll();
        }
    }
}
