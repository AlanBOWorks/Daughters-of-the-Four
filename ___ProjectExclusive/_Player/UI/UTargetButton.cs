using System;
using _CombatSystem;
using Characters;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Player
{
    public class UTargetButton : MonoBehaviour, IPointerClickHandler
    {
        private CombatingEntity _currentEntity; //TODO inject this
        public void Injection(CombatingEntity entity) => _currentEntity = entity;

        public void Show()
        {
            //TODO make animation?
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var button = eventData.button;
            Debug.Log("Click Target");
            switch (button)
            {
                case PointerEventData.InputButton.Left:
                    PlayerEntitySingleton.TargetsHandler.HideSkillTargets();
                    PlayerEntitySingleton.SkillButtonsHandler.HideButtons();
                    CombatSystemSingleton.actionSkillHandler.DoSkill(_currentEntity);
                    break;
                    
                default:
                    break;
            }
        }
    }
}
