using System;
using CombatSystem.Localization;
using CombatSystem.Skills;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

namespace CombatSystem.UI
{
    public class USkillElementHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] 
        private Image iconHolder;

        [SerializeField] 
        private TextMeshProUGUI costHolder;

        [ShowInInspector, HideInEditorMode]
        public IFullSkill CurrentSkill { get; private set; }

        private ISkillElementEventsHandler _eventsHandler;

        public void Injection(IFullSkill skill)
        {
            CurrentSkill = skill;
            UpdateCost();
            UpdateIcon();
        }

        public void Injection(ISkillElementEventsHandler handler)
        {
            _eventsHandler = handler;
        }

        private void UpdateIcon()
        {
            iconHolder.sprite = CurrentSkill.GetSkillIcon();
        }
        private void UpdateCost()
        {
            var targetCost = LocalizeSkills.LocalizeSkillCost(CurrentSkill);
            costHolder.text = targetCost;
        }




        public interface ISkillElementEventsHandler
        {
            void OnPointerEnter(IFullSkill skill);
            void OnPointerExit(IFullSkill skill);
            void OnPointerClick(IFullSkill skill);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _eventsHandler?.OnPointerClick(CurrentSkill);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _eventsHandler?.OnPointerExit(CurrentSkill);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _eventsHandler?.OnPointerClick(CurrentSkill);
        }
    }
}
