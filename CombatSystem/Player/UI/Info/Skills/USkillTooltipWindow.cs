using System;
using System.Collections.Generic;
using CombatSystem.Skills;
using Exoa.Responsive;
using MEC;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Utils;
using Utils.Utils;

namespace CombatSystem.Player.UI
{
    public class USkillTooltipWindow : MonoBehaviour
    {
        [Title("Pool References")]
        [SerializeField, PropertyOrder(10)] private ToolTipWindowPool pool;

        [Title("Mono References")] 
        [SerializeField]
        private RectTransform parentHolder;

        private void Awake()
        {
            pool.Awake();
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Timing.KillCoroutines(_coroutineHandle);
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
            HandleText(in textElement, in values);
        }

        private static void HandleText(in TextMeshProUGUI text, in PerformEffectValues values)
        {
            var effectText = Localization.LocalizeEffects.LocalizeEffectTooltip(in values);
            text.text = effectText;
        }

        private float _accumulatedHeight;
        private void HandleHeight(in TextMeshProUGUI text)
        {
            var textTransform = text.rectTransform;
            float textHeight = text.preferredHeight;

            float pivotHeight = _accumulatedHeight;
            UtilsRectTransform.SetPivotVertical(in textTransform, -pivotHeight);

            _accumulatedHeight += textHeight;
        }

        private CoroutineHandle _coroutineHandle;
        public void OnFinisHandlingEffects()
        {
            bool isActive = pool.IsActive();

            if (isActive)
            {
                Timing.KillCoroutines(_coroutineHandle);
                _coroutineHandle = Timing.RunCoroutine(_HeightResize());
                Show();
            }
            else
            {
                Hide();
            }
        }

        // PROBLEM: content Size Fitter updates the size in LateUpdate (source needed), making the real size incorrect
        // at Update time.
        //
        // SOLUTION: force a wait through a coroutine
        private IEnumerator<float> _HeightResize()
        {
            _accumulatedHeight = 0;
            yield return Timing.WaitForOneFrame;
            var activeElements = pool.GetActiveElements();
            foreach (var element in activeElements)
            {
                HandleHeight(in element);
            }
            parentHolder.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _accumulatedHeight);
        }

        [Serializable]
        private sealed class ToolTipWindowPool : TrackedMonoObjectPool<TextMeshProUGUI>
        {
            
        }
    }
}
