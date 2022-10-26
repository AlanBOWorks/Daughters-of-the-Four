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

        [SerializeField] 
        private RectTransform onActiveBackground;

        [ShowInInspector, HideInEditorMode]
        public IFullSkill CurrentSkill { get; private set; }

        private ISkillElementEventsHandler _eventsHandler;

        public Image GetIconHolder() => iconHolder;
        public RectTransform GetIconBackgroundColor() => onActiveBackground;

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
            void OnPointerEnter(USkillElementHolder element, IFullSkill skill);
            void OnPointerExit(USkillElementHolder element, IFullSkill skill);
            void OnPointerClick(USkillElementHolder element, IFullSkill skill);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _eventsHandler?.OnPointerEnter(this,CurrentSkill);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _eventsHandler?.OnPointerExit(this, CurrentSkill);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _eventsHandler?.OnPointerClick(this, CurrentSkill);
        }
    }
}
