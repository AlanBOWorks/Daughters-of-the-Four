using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using MEC;
using Sirenix.OdinInspector;
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

        private CoroutineHandle _fadeHandle;
        private const float FadeSpeed = 8f;

        private ISkillButtonListener _holder;

        [ShowInInspector]
        private CombatSkill _skill;

        internal void Injection(in ISkillButtonListener holder)
        {
            _holder = holder;
        }
        internal void Injection(in CombatSkill skill)
        {
            _skill = skill;
            iconHolder.sprite = skill.Preset.GetSkillIcon();
        }


        internal void ShowButton()
        {
            gameObject.SetActive(true);

            _fadeHandle = Timing.RunCoroutine(_FadeAlpha());
            CombatSystemSingleton.LinkCoroutineToMaster(_fadeHandle);
            IEnumerator<float> _FadeAlpha()
            {
                canvasGroup.alpha = 0;
                while (canvasGroup.alpha < .98f)
                {
                    canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, Time.deltaTime * FadeSpeed);

                    yield return Timing.WaitForOneFrame;
                }

                canvasGroup.alpha = 1;
            }
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
            _holder.OnSkillSelect(in _skill);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _holder.OnSkillButtonHover(in _skill);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _holder.OnSkillButtonExit(in _skill);
        }
    }
}
