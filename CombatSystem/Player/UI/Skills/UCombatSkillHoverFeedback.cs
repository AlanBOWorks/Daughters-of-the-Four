using System;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public sealed class UCombatSkillHoverFeedback : MonoBehaviour, ISkillButtonListener
    {
        [Title("References")] 
        [SerializeField] private UCombatSkillButtonsHolder skillButtonsHolder;

        [TitleGroup("Images")]
        [SerializeField] private RectTransform hoverHolder;
        [SerializeField] private RectTransform focusHolder;


        private Quaternion _hoverInitialRotation;
        private Quaternion _focusInitialRotation;
        private void Start()
        {
            PlayerCombatSingleton.PlayerCombatEvents.SubscribeAsPlayerEvent(this);

            _hoverInitialRotation = hoverHolder.localRotation;
            _focusInitialRotation = focusHolder.localRotation;

            hoverHolder.gameObject.SetActive(false);
            focusHolder.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }

        private const float AnimationDuration = .08f;
        private static void DoAnimate(in RectTransform imageHolder, in UCombatSkillButton onButton, in Quaternion targetRotation)
        {
            imageHolder.gameObject.SetActive(true);
            imageHolder.position = onButton.transform.position;

            DOTween.Kill(imageHolder);
            imageHolder.DOLocalRotateQuaternion(targetRotation, AnimationDuration);
        }

        private static void Hide(in RectTransform imageHolder)
        {
            imageHolder.gameObject.SetActive(false);
        }


        public void OnSkillButtonHover(in CombatSkill skill)
        {
            var targetButton = skillButtonsHolder.GetDictionary()[skill];

            hoverHolder.localRotation = Quaternion.AngleAxis(-10, Vector3.forward);
            DoAnimate(in hoverHolder,in targetButton, in _hoverInitialRotation);
        }

        public void OnSkillButtonExit(in CombatSkill skill)
        {
            Hide(in hoverHolder);
        }


        public void OnSkillSelect(CombatSkill skill)
        {

        }

        public void OnSkillSelectFromNull(CombatSkill skill)
        {
        }

        public void OnSkillSwitch(CombatSkill skill, CombatSkill previousSelection)
        {
            var targetButton = skillButtonsHolder.GetDictionary()[skill];

            focusHolder.localRotation = _hoverInitialRotation;
            DoAnimate(in focusHolder,in targetButton, in _focusInitialRotation);
        }

        public void OnSkillDeselect(CombatSkill skill)
        {
            Hide(in focusHolder);
        }

        public void OnSkillCancel(CombatSkill skill)
        {
            Hide(in focusHolder);
        }

        public void OnSkillSubmit(CombatSkill skill)
        {
            Hide(in focusHolder);
        }
    }
}
