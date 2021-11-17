using System;
using CombatEntity;
using CombatTeam;
using System.Collections;
using System.Collections.Generic;
using CombatCamera;
using CombatSystem;
using MEC;
using UnityEngine;


namespace __ProjectExclusive.Player.UI
{
    public class UTargetButtonsHolder : UCanvasPivotOverEntityListener, IVirtualSkillInteraction, 
        ICombatDisruptionListener
    {
        private void Start()
        {

            PlayerCombatSingleton.PlayerEvents.Subscribe(this);
            CombatSystemSingleton.CombatPreparationHandler.Subscribe(this);

        }
       

        private Dictionary<CombatingEntity, UPivotOverEntity> _entitiesTracker;


        public override void OnPooledElement(CombatingEntity user, UPivotOverEntity pivotOverEntity)
        {
            var pivotReferences = pivotOverEntity.GetReferences();
            var targetButton = pivotReferences.GetTargetButton();
            targetButton.Injection(this);
            targetButton.Injection(user);
            targetButton.Hide();
        }

        public override void InjectDictionary(Dictionary<CombatingEntity, UPivotOverEntity> dictionary)
        {
            _entitiesTracker = dictionary;
        }

        public void OnCombatPause()
        {
            throw new NotImplementedException();
        }

        public void OnCombatResume()
        {
            throw new NotImplementedException();
        }

        public void OnCombatExit()
        {
        }

        private void Show(VirtualSkillSelection selection)
        {
            var possibleTargets = selection.PossibleTargets;
            foreach (CombatingEntity target in possibleTargets)
            {
                var element = _entitiesTracker[target];
                var targetHolder = GetButton(element);
                targetHolder.Show();
            }
        }
        private void Hide(VirtualSkillSelection selection)
        {
            var possibleTargets = selection.PossibleTargets;
            foreach (CombatingEntity target in possibleTargets)
            {
                var element = _entitiesTracker[target];
                var targetHolder = GetButton(element);
                targetHolder.Hide();
            }
        }


        public void OnSelect(VirtualSkillSelection selection)
        {
            Show(selection);
        }

        public void OnDeselect(VirtualSkillSelection selection)
        {
            Hide(selection);
        }

        public void OnSubmit(VirtualSkillSelection selection)
            => OnDeselect(selection);

        public void OnHover(VirtualSkillSelection selection)
        {
            //Todo show as Hover
        }

        public void OnHoverExit(VirtualSkillSelection selection)
        {
            //Todo hide as Hover
        }

        public void OnTargetSelect(UTargetButton button)
        {
            DisableButtons();
            SendValues();
            Timing.RunCoroutine(
                _DisableAnimation(), Segment.RealtimeUpdate);
            

            void DisableButtons()
            {
                foreach (var pair in _entitiesTracker)
                {
                    var targetHolder = GetButton(pair.Value);
                    targetHolder.enabled = false;
                }
            }
            void SendValues()
            {
                var selectedTarget = button.GetEntity();
                PlayerCombatSingleton.PlayerEvents.OnVirtualTargetSelect(selectedTarget);
            }
            IEnumerator<float> _DisableAnimation()
            {
                //Just a small offset (.04f) so the eye can sense the ending a little better
                yield return Timing.WaitForSeconds(UTargetButton.PointerClickAnimationDuration + .04f);
                foreach (var pair in _entitiesTracker)
                {
                    var targetHolder = GetButton(pair.Value);
                    targetHolder.Hide();
                }
            }
        }

        private UTargetButton GetButton(UPivotOverEntity pivot) => pivot.GetReferences().GetTargetButton();
    }
}
