using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using CombatSystem.Stats;
using CombatSystem.Team;
using MEC;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
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
        
        [SerializeField] 
        private SkillInfoHandler skillInfo = new SkillInfoHandler();

        [Title("Params")] 
        [SerializeField,SuffixLabel("px")] private float topMargin = 12;
        [SerializeField,SuffixLabel("px")] private float bottomMargin = 12;

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



        public void HandleSkill(ICombatSkill skill)
        {
            skillInfo.HandleEffect(skill);
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
            _accumulatedHeight = topMargin;
            yield return Timing.WaitForOneFrame;
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
        private sealed class SkillInfoHandler
        {
            [SerializeField] private TextMeshProUGUI nameHolder;
            [SerializeField] private Image roleIconHolder;

            public void HandleEffect(ICombatSkill skill)
            {
                var skillName = LocalizeSkills.LocalizeSkill(skill);
                nameHolder.text = skillName;

                var archetype = skill.TeamTargeting;
                var roleThemesHolder = CombatThemeSingleton.SkillsThemeHolder;
                var roleTheme = UtilsSkill.GetElement(archetype, roleThemesHolder);
                var roleColor = roleTheme.GetThemeColor();
                var roleIcon = roleTheme.GetThemeIcon();

                roleIconHolder.sprite = roleIcon;
                roleIconHolder.color = roleColor;
            }
        }


        [Serializable]
        private sealed class ToolTipWindowPool : TrackedMonoObjectPool<UEffectTooltipHolder>
        {
            
        }
    }
}
