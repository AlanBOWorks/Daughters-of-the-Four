using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using MEC;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CombatSystem.Player.UI
{
    public class UCombatSkillButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] 
        private CanvasGroup canvasGroup;
        [SerializeField] 
        private Image iconHolder;
        [SerializeField] 
        private TextMeshProUGUI costText;

        private CoroutineHandle _fadeHandle;
        private const float FadeSpeed = 8f;

        private UCombatSkillButtonsHolder _holder;

        [ShowInInspector,DisableInEditorMode]
        private CombatSkill _skill;

        private bool _canSubmitSkill;

        internal void Injection(in UCombatSkillButtonsHolder holder)
        {
            _holder = holder;
        }
        internal void Injection(CombatSkill skill)
        {
            _skill = skill;
            var preset = skill.Preset;

            UpdateIcon();
            UpdateCostReal();



            void UpdateIcon()
            {
                var icon = preset.GetSkillIcon();
                if(icon) iconHolder.sprite = icon;
            }
            
        }

        private const string OverflowCostText = "?";
        public void UpdateCostReal()
        {
            var costAmount = _skill.SkillCost;
            var skillCostString = costAmount > 9 
                ? OverflowCostText 
                : costAmount.ToString();

            costText.text = skillCostString;
        }

        private void OnDestroy()
        {
            Timing.KillCoroutines(_fadeHandle);
        }

        private void OnEnable()
        {
            Timing.ResumeCoroutines(_fadeHandle);
        }

        private void OnDisable()
        {
            Timing.PauseCoroutines(_fadeHandle);
        }

        private void AnimateButton(float targetAlpha)
        {

            Timing.KillCoroutines(_fadeHandle);
            _fadeHandle = Timing.RunCoroutine(_FadeAlpha());
            IEnumerator<float> _FadeAlpha()
            {
                canvasGroup.alpha = 0;
                while (canvasGroup.alpha < targetAlpha)
                {
                    canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, Time.deltaTime * FadeSpeed);

                    yield return Timing.WaitForOneFrame;
                }

                canvasGroup.alpha = targetAlpha;
            }
        }

        internal void DoShowActiveButton()
        {
            _canSubmitSkill = true;

            gameObject.SetActive(true);
            enabled = true;
            AnimateButton(1);
        }

        private const float DisableAlpha = .3f;
        internal void DoShowDisabledButton()
        {
            _canSubmitSkill = false;

            gameObject.SetActive(true);
            enabled = true;
            AnimateButton(DisableAlpha);
        }

        internal void HideButton()
        {
            Timing.KillCoroutines(_fadeHandle);
            gameObject.SetActive(false);
            canvasGroup.alpha = 0;
        }

       


        public void SelectButton()
        {

        }
        public void DeSelectButton()
        {

        }

        internal void ResetState()
        {
            _skill = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(!_canSubmitSkill) return;

            _holder.DoSkillSelect(_skill);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _holder.DoSkillButtonHover(_skill);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _holder.DoSkillButtonExit(_skill);
        }
    }
}
