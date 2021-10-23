using System;
using CombatEntity;
using CombatTeam;
using System.Collections;
using System.Collections.Generic;
using CombatCamera;
using CombatSystem;
using UnityEngine;


namespace __ProjectExclusive.Player
{
    public class UTargetButtonsHolder : UPersistentTeamStructurePoolerBase<UTargetButton>, IVirtualSkillInteraction,
        ICombatCameraSwitchListener
    {
        private void Start()
        {
            _entitiesTracker = new Dictionary<CombatingEntity, UTargetButton>();

            CombatCameraSingleton.SubscribeListener(this);
            PlayerCombatSingleton.PlayerEvents.Subscribe(this);
        }


        private void OnDestroy()
        {
            CombatCameraSingleton.UnSubscribe(this);
        }

        private Dictionary<CombatingEntity, UTargetButton> _entitiesTracker;
        private Camera _referenceCamera;

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
            instantiatedElement.Injection(_referenceCamera);
            instantiatedElement.Hide();
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



        private void Show(IVirtualSkillSelection selection)
        {
            var possibleTargets = selection.PossibleTargets;
            foreach (CombatingEntity target in possibleTargets)
            {
                _entitiesTracker[target].Show();
            }
        }
        private void Hide(IVirtualSkillSelection selection)
        {
            var possibleTargets = selection.PossibleTargets;
            foreach (CombatingEntity target in possibleTargets)
            {
                _entitiesTracker[target].Hide();
            }
        }

        private IVirtualSkillSelection _skillSelectionClick;
        private IVirtualSkillSelection _skillSelectionHover;

        public void OnSelect(IVirtualSkillSelection selection)
        {
            Show(selection);
            _skillSelectionClick = selection;
        }

        public void OnDeselect(IVirtualSkillSelection selection)
        {
            Hide(selection);
            _skillSelectionClick = null;
        }

        public void OnSubmit(IVirtualSkillSelection selection)
            => OnDeselect(selection);

        public void OnHover(IVirtualSkillSelection selection)
        {
            //Todo show as Hover
            _skillSelectionHover = selection;
        }

        public void OnHoverExit(IVirtualSkillSelection selection)
        {
            //Todo hide as Hover
            _skillSelectionHover = null;
        }

        public void OnMainCameraSwitch(Camera mainCamera)
        {
            _referenceCamera = mainCamera;
            foreach (var pair in _entitiesTracker)
            {
                pair.Value.Injection(_referenceCamera);
            }
        }

        public void OnUICameraSwitch(Camera uiCamera)
        {
            
        }
    }
}
