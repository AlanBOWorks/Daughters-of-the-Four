using System;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UActiveMembersManager : MonoBehaviour, ITempoControlStatesListener,
        IPlayerCombatEventListener
    {
        [SerializeField] private UUIHeadHoverEntityHandler hoverHeadHandler;
        [Title("Had Params")]
        [SerializeField] private RectTransform activeMemberToggle;
        [SerializeField] private Vector2 headUIPositionOffset;

        private void Awake()
        {
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
        }

        private void OnDisable()
        {
            HideControl();
        }

        private void ShowControl(CombatEntity targetEntity)
        {
            var dictionary = hoverHeadHandler.GetDictionary();
            var element = dictionary[targetEntity];

            activeMemberToggle.SetParent(element.transform);
            activeMemberToggle.localPosition = headUIPositionOffset;
            activeMemberToggle.gameObject.SetActive(true);
        }
        private void HideControl()
        {
            activeMemberToggle.gameObject.SetActive(false);
            activeMemberToggle.SetParent(transform);
        }

        public void OnTempoStartControl(CombatTeamControllerBase controller, CombatEntity firstControl)
        {
        }

        public void OnAllActorsNoActions(CombatEntity lastActor)
        {
            HideControl();
        }

        public void OnTempoFinishControl(CombatTeamControllerBase controller)
        {
            HideControl();
        }

        public void OnPerformerSwitch(CombatEntity performer)
        {
            ShowControl(performer);
        }

        public void OnTeamStancePreviewSwitch(EnumTeam.StanceFull targetStance)
        {
        }
    }
}
