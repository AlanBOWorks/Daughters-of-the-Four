using System;
using CombatSystem.Entity;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player
{
    public class UFinishSequenceButton : UHoldButton
    {
        [SerializeField] private CanvasGroup alphaGroup;

        private void Start()
        {
            Disable();
        }

        protected static void DoFinishSequenceAction()
        {
            PlayerCombatSingleton.PlayerTeamController.FinishCurrentPerformer();
        }

        public override void OnTempoStartControl(CombatTeamControllerBase controller, CombatEntity firstControl)
        {
            base.OnTempoStartControl(controller, firstControl);
            bool isSingleActor = controller.GetAllControllingMembers().Count <= 1;
            if (isSingleActor) return;

            Enable();
        }

        public override void OnTempoFinishControl(CombatTeamControllerBase controller)
        {
            Disable();
            base.OnTempoFinishControl(controller);
        }

        private void Enable()
        {
            enabled = true;
            alphaGroup.alpha = 1;
        }

        private const float DisableAlphaValue = .3f;
        private void Disable()
        {
            alphaGroup.alpha = DisableAlphaValue;
            enabled = false;
        }
    }
}
