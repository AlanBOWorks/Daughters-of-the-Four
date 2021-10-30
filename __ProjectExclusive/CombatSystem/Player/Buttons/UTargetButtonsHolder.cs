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
    public class UTargetButtonsHolder : UPersistentTeamStructurePoolerBase<UTargetButton>, IVirtualSkillInteraction
    {
        private void Start()
        {
            _entitiesTracker = new Dictionary<CombatingEntity, UTargetButton>();

            PlayerCombatSingleton.PlayerEvents.Subscribe(this);
        }



        private Dictionary<CombatingEntity, UTargetButton> _entitiesTracker;

        public override void OnPreparationCombat(CombatingTeam playerTeam, CombatingTeam enemyTeam)
        {
            _entitiesTracker.Clear();
            base.OnPreparationCombat(playerTeam, enemyTeam);

        }

        public override void OnAfterLoads()
        {
        }

        protected override void OnPoolElement(ref UTargetButton instantiatedElement)
        {
            instantiatedElement.Hide();
            instantiatedElement.Injection(this);
        }

        protected override void OnPreparationEntity(CombatingEntity entity, UTargetButton element)
        {
            _entitiesTracker.Add(entity,element);
            element.Injection(entity);
        }

        public override void OnFinish(CombatingTeam wonTeam)
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
