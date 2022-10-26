using CombatSystem.Localization;
using CombatSystem.Player.UI;
using CombatSystem.Team;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CombatSystem.UI
{
    public class UStanceElementHolder : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] 
        private RectTransform animationTarget;
        [SerializeField] 
        private TextMeshProUGUI stanceNameHolder;

        [SerializeField]
        private Image backgroundHolder;
        [SerializeField] 
        private Image stanceIcon;

        public IStanceEventsHandler EventsHandler { set; private get; }

        public RectTransform GetAnimationTarget() => animationTarget;
        public TextMeshProUGUI GetTextHolder() => stanceNameHolder;
        public Image GetBackgroundHolder() => backgroundHolder;
        public Image GetIconHolder() => stanceIcon;

        public EnumTeam.Stance CurrentStance { get; private set; }
        public void Injection(EnumTeam.Stance stance)
        {
            CurrentStance = stance;
            UpdateStanceName();
        }

        private void UpdateStanceName()
        {
            var stanceName = LocalizationsCombat.LocalizeStance(CurrentStance);
            stanceNameHolder.text = stanceName;
            UpdateStanceVisuals();
        }

        private void UpdateStanceVisuals()
        {
            var theme = CombatThemeSingleton.RolesThemeHolder;
            var themeElement = UtilsTeam.GetElement(CurrentStance, theme);
            stanceIcon.sprite = themeElement.GetThemeIcon();

            var stanceColor = themeElement.GetThemeColor();
            stanceColor.a = 0;
            stanceIcon.color = stanceColor;
        }

        public interface IStanceEventsHandler
        {
            void OnPointerEnter(UStanceElementHolder holder, EnumTeam.Stance stance);
            void OnPointerExit(UStanceElementHolder holder, EnumTeam.Stance stance);
            void OnPointerClick(UStanceElementHolder holder, EnumTeam.Stance stance);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            EventsHandler?.OnPointerClick(this, CurrentStance);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            EventsHandler?.OnPointerEnter(this, CurrentStance);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            EventsHandler?.OnPointerExit(this, CurrentStance);
        }
    }
}
