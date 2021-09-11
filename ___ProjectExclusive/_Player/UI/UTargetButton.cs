using System;
using _CombatSystem;
using Characters;
using DG.Tweening;
using Skills;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Player
{
    public class UTargetButton : MonoBehaviour, IPointerClickHandler, IEntitySwitchListener
    {
        private CombatingEntity _currentTarget; //TODO inject this
        [SerializeField] private Image button;
        private Tweener _currentTween;

        public void Show()
        {
            gameObject.SetActive(true);

            button.transform.DOPunchScale(new Vector3(1.01f, 1.01f, 1.01f), .2f, 1,0.5f);
            _currentTween = button.DOFade(1, .3f);
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            if (_currentTween != null) 
                DOTween.Kill(_currentTween);

            button.transform.localScale = Vector3.one;
            button.DOFade(0, .3f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var button = eventData.button;
            switch (button)
            {
                case PointerEventData.InputButton.Left:
                    var currentEntity = TempoHandler.CurrentActingEntity;
                    var currentSkill = PlayerEntitySingleton.SkillsTracker.CurrentSelectedSkill;

                    PlayerEntitySingleton.SkillButtonsHandler.OnSubmitSkill();
                    CombatSystemSingleton.PerformSkillHandler.DoSkill(currentSkill,currentEntity,_currentTarget);
                    break;
                    //TODO right click?
            }
        }

        public void OnEntitySwitch(CombatingEntity entity)
        {
            _currentTarget = entity;
        }
    }
}
