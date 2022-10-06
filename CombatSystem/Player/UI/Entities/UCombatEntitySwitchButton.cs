using System;
using CombatSystem.Entity;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils.UI;
using Utils_Project;

namespace CombatSystem.Player.UI
{
    public class UCombatEntitySwitchButton : UButtonPointerFeedback
    {
        [SerializeField] private UCombatEntitySwitcherHandler switcherHandler;
        [Title("Colors")]
        [SerializeField] private Image iconHolder;
        [SerializeReference] private ColorsHolder colors = new ColorsHolder();
        

        [Title("Elements")]
        [SerializeField]
        private Image[] changeColorOnActive = new Image[0];
        [SerializeField]
        private GameObject[] toggleOnEntityActive = new GameObject[0];


        [Title("ActionsAmount")]
        [SerializeField]
        private CurrentActionsHolder actionsAmountTextHandler = new CurrentActionsHolder();




        public Image GetIconHolder() => iconHolder;


        private CombatEntity _user;
        public void Injection(in CombatEntity entity)
        {
            _user = entity;
        }

        public void Injection(in Sprite icon)
        {
            iconHolder.sprite = icon;
        }

        public void DoEnable(bool enableButton)
        {
            actionsAmountTextHandler.SetActive(enableButton);

            var targetColor = colors.GetColor(enableButton);
            foreach (var image in changeColorOnActive)
            {
                image.color = targetColor;
            }

            foreach (var element in toggleOnEntityActive)
            {
                element.SetActive(enableButton);
            }

            if(!enableButton) return;

           UpdateActionsInfo();
        }

        public void OnNullEntity()
        {
            enabled = false;
            _user = null;
            DoEnable(false);
        }


        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if(_user == null) return;

            switcherHandler.DoSwitchEntity(_user);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            switcherHandler.OnHoverEnter(iconHolder);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            switcherHandler.OnHoverExit();
        }

        public void UpdateCurrentActionsAmount()
        {
            float currentActionsAmount = _user.Stats.UsedActions;
            actionsAmountTextHandler.UpdateCurrentActionsText(currentActionsAmount);
        }

        public void UpdateMaxActionsAmount()
        {
            float maxActionsAmount = UtilsCombatStats.CalculateActionsLimitRounded(_user.Stats);
            actionsAmountTextHandler.UpdateMaxActionsText(maxActionsAmount);
        }

        public void UpdateActionsInfo()
        {
            UpdateCurrentActionsAmount();
            UpdateMaxActionsAmount();
        }


        [Serializable]
        private sealed class ColorsHolder
        {
            [SerializeField]
            private Color onActiveColor;
            [SerializeField]
            private Color onDisableColor;

            public Color GetColor(bool isEntityActive) => isEntityActive ? onActiveColor : onDisableColor;
        }

        [Serializable]
        private sealed class CurrentActionsHolder
        {
            [SerializeField] private TextMeshProUGUI actionsAmountText;
            [SerializeField] private TextMeshProUGUI maxActionsAmount;

            public void SetActive(bool active)
            {
                actionsAmountText.transform.parent.gameObject.SetActive(active);
            }
            public void UpdateCurrentActionsText(float current)
            {
                string text = ConvertAmount(current);
                actionsAmountText.text = text;
            }

            public void UpdateMaxActionsText(float maxAmount)
            {
                string text = ConvertAmount(maxAmount);
                maxActionsAmount.text = text;
            }

            private const string OverFlowText = "XX";
            private static string ConvertAmount(float amount)
            {
                return amount > 99 
                    ? OverFlowText 
                    : amount.ToString("00");
            }
        }
    }
}
