using System;
using System.Collections.Generic;
using CombatSystem.Localization;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;
using Utils.Utils;

namespace CombatSystem.Player.UI
{
    public class UEffectsTooltipWindowHandler : MonoBehaviour
    {
        [Title("Pool References")]
        [SerializeField, PropertyOrder(10)] private ToolTipWindowPool pool;

        [Title("Mono References")] 
        [SerializeField]
        private RectTransform parentHolder;
        

        [Title("Params")] 
        [SerializeField,SuffixLabel("px")] private float topMargin = 12;
        [SerializeField,SuffixLabel("px")] private float bottomMargin = 12;

        private void Awake()
        {
            pool.Awake();
            gameObject.SetActive(false);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            pool.ReturnToElementsToPool();
        }

        public void Show()
        {
            if(gameObject.activeSelf) return;
            gameObject.SetActive(true);

            //todo animation?
        }



        public void HandleEffect(in PerformEffectValues values)
        {
            var textElement = pool.Get();
            HandleText(textElement, in values);
            HandleIcon(textElement,values.Effect);
        }
        private static void HandleText(UEffectTooltipHolder holder, in PerformEffectValues values)
        {
            var text = holder.GetTextHolder();
            var effectText = LocalizeEffects.LocalizeEffectTooltip(in values);
            text.text = effectText;
        }

        public static void HandleIcon(UEffectTooltipHolder holder, IEffectBasicInfo effect)
        {
            var icon = UtilsVisual.GetEffectSprite(effect);
            var iconHolder = holder.GetIconHolder();
            iconHolder.sprite = icon;
        }


        private float _accumulatedHeight;
        public void OnFinisHandlingEffects()
        {
            bool isActive = pool.IsActive();

            if (isActive)
            {
                HeightResize();
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void HeightResize()
        {
            _accumulatedHeight = topMargin;
            var activeElements = pool.GetActiveElements();
            foreach (var element in activeElements)
            {
                HandleHeight(in element);
            }

            _accumulatedHeight += bottomMargin;
            parentHolder.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _accumulatedHeight);
        }
        private void HandleHeight(in UEffectTooltipHolder holder)
        {
            var text = holder.GetTextHolder();
            var textTransform = text.rectTransform;
            float textHeight = text.preferredHeight;

            float pivotHeight = _accumulatedHeight;
            UtilsRectTransform.SetPivotVertical(in textTransform, -pivotHeight);

            _accumulatedHeight += textHeight;
        }



        [Serializable]
        private sealed class ToolTipWindowPool : TrackedMonoObjectPool<UEffectTooltipHolder>
        {
            
        }
    }
}
