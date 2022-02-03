using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using DG.Tweening;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class USkillButtonsHolder : MonoBehaviour, ITempoEntityStatesListener
    {
        [Title("References")]
        [SerializeField] 
        private UCombatSkillButton clonableSkillButton;
        private Stack<UCombatSkillButton> _buttonsStack;

        [Title("Parameters")]
        [SerializeField]
        private Vector2 buttonsSeparations;
        private Vector2 _buttonSizes;

        private void Awake()
        {
            var buttonTransform = (RectTransform) clonableSkillButton.transform;
            _buttonSizes = buttonTransform.rect.size;
            _buttonSizes.y = 0; // This is just to avoid the buttons moving upwards in ShowSkills

            _buttonsStack = new Stack<UCombatSkillButton>();
        }

        private void Start()
        {
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
        }

        // Safe check
        private const int MaxSkillAmount = 12;
        private void HandlePool(int skillsAmount)
        {
            if (skillsAmount > MaxSkillAmount) skillsAmount = MaxSkillAmount;

            while (skillsAmount > _buttonsStack.Count)
            {
                UCombatSkillButton generated = Instantiate(clonableSkillButton, transform);
                _buttonsStack.Push(generated);
            }
        }

        private const float DelayBetweenButtons = .12f;
        private const float AnimationDuration = .2f;
        [Button,DisableInEditorMode]
        private void ShowSkillsAnimated()
        {
            CombatSystemSingleton.LinkCoroutineToMaster(_ShowAll());
            IEnumerator<float> _ShowAll()
            {
                int index = 0;
                Vector2 lastPoint = Vector2.zero;
                foreach (var button in _buttonsStack)
                {
                    yield return Timing.WaitForSeconds(DelayBetweenButtons);

                    var buttonTransform = (RectTransform) button.transform;


                    Vector2 targetPoint = index * (buttonsSeparations + _buttonSizes);

                    buttonTransform.localPosition = lastPoint;
                    buttonTransform.DOLocalMove(targetPoint, AnimationDuration);
                    button.ShowButton();


                    lastPoint = targetPoint;
                    index++;
                }
            }
        }

        public void OnEntityRequestSequence(CombatEntity entity, bool canAct)
        {
        }

        public void OnEntityRequestControl(CombatEntity entity)
        {
            var entitySkills = entity.GetCurrentSkills();
            int skillAmount = entitySkills.Count;
            HandlePool(skillAmount);
            ShowSkillsAnimated();
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
        }

        public void OnEntityFinishSequence(CombatEntity entity)
        {
        }
    }
}
