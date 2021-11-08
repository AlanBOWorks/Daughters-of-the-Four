using System;
using CombatEntity;
using CombatTeam;
using System.Collections;
using System.Collections.Generic;
using CombatCamera;
using CombatSystem;
using MEC;
using UnityEngine;


namespace __ProjectExclusive.Player
{
    public class UTargetButtonsHolder : MonoBehaviour, IVirtualSkillInteraction, ICanvasPivotOverEntityListener,
        ICombatDisruptionListener
    {
        private void Start()
        {
            _entitiesTracker = new Dictionary<CombatingEntity, UTargetButton>();

            PlayerCombatSingleton.PlayerEvents.Subscribe(this);
            CombatSystemSingleton.CombatPreparationHandler.Subscribe(this);

            subscribeToCanvasPooler.PoolListeners.Add(this);
        }
        [SerializeField] 
        private UCanvasPivotOverEntities subscribeToCanvasPooler;

        private Dictionary<CombatingEntity, UTargetButton> _entitiesTracker;


        public void OnPooledElement(CombatingEntity user, UPivotOverEntity pivotOverEntity)
        {
            var pivotReferences = pivotOverEntity.GetReferences();
            var targetButton = pivotReferences.GetTargetButton();
            _entitiesTracker.Add(user, targetButton);
            targetButton.Injection(this);
            targetButton.Injection(user);
            targetButton.Hide();
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
            _entitiesTracker.Clear();
        }

        private void Show(VirtualSkillSelection selection)
        {
            var possibleTargets = selection.PossibleTargets;
            foreach (CombatingEntity target in possibleTargets)
            {
                _entitiesTracker[target].Show();
            }
        }
        private void Hide(VirtualSkillSelection selection)
        {
            var possibleTargets = selection.PossibleTargets;
            foreach (CombatingEntity target in possibleTargets)
            {
                _entitiesTracker[target].Hide();
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
                foreach (KeyValuePair<CombatingEntity, UTargetButton> pair in _entitiesTracker)
                {
                    pair.Value.enabled = false;
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
                foreach (KeyValuePair<CombatingEntity, UTargetButton> pair in _entitiesTracker)
                {
                    pair.Value.Hide();
                }
            }
        }


    }
}
