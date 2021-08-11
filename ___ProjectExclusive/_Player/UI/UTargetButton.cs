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
    public class UTargetButton : MonoBehaviour, IPointerClickHandler
    {
        private CombatingEntity _currentTarget; //TODO inject this
        private Image _button;
        private Tweener _currentTween;


        public void Injection(CombatingEntity entity)
        {
            _currentTarget = entity;
            if (_button != null) return;
            _button = GetComponent<Image>();
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);

            _button.transform.DOPunchScale(new Vector3(1.01f, 1.01f, 1.01f), .2f, 1,0.5f);
            _currentTween = _button.DOFade(1, .3f);
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            if (_currentTween != null) 
                DOTween.Kill(_currentTween);

            _button.transform.localScale = Vector3.one;
            _button.DOFade(0, .3f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var button = eventData.button;
            switch (button)
            {
                case PointerEventData.InputButton.Left:
                    PlayerEntitySingleton.SkillButtonsHandler.OnSubmitSkill();
                    PerformSkillHandler.SendDoSkill(_currentTarget);
                    break;
                    //TODO left click?
                default:
                    break;
            }
        }
    }
}
