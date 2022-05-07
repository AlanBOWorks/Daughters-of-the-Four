using System;
using CombatSystem.Entity;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.Actions
{
    public class UFinishSequenceButton : UHoldButton
    {
        [SerializeField] private CanvasGroup alphaGroup;

        private void Start()
        {
            Disable();
        }

        protected override void DoAction()
        {
            PlayerCombatSingleton.PlayerTeamController.FinishCurrentPerformer();
        }

        public override void OnTempoStartControl(in CombatTeamControllerBase controller)
        {
            base.OnTempoStartControl(in controller);

            bool isSingleActor = controller.GetAllControllingMembers().Count <= 1;
            if(isSingleActor) return;
            
            Enable();
        }

        public override void OnControlFinishAllActors(in CombatEntity lastActor)
        {
            Disable();
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
