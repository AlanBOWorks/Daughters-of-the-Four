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

        public override void OnTempoPreStartControl(CombatTeamControllerBase controller)
        {
            base.OnTempoPreStartControl(controller);

            bool isSingleActor = controller.GetAllControllingMembers().Count <= 1;
            if(isSingleActor) return;
            
            Enable();
        }

        public override void OnControlFinishAllActors(CombatEntity lastActor)
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
