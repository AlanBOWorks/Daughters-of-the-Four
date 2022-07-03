using System;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using DG.Tweening;
using Localization.Characters;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem.Player.UI
{
    public class UTempoTrackerHolder : MonoBehaviour, IEntityExistenceElement<UTempoTrackerHolder>
    {
        [Title("Holder")]
        [SerializeField] private CanvasGroup alphaGroup;


        [Title("Elements")]
        [SerializeField] private TextMeshProUGUI entityName;
        [SerializeField] private TextMeshProUGUI currentTick;
        [SerializeField] private TextMeshProUGUI entitySpeed;
        [SerializeField] private TextMeshProUGUI remainingStepsText;
        [SerializeField] private Image roleIcon;

        [ShowInInspector,DisableInEditorMode]
        private CombatEntity _user;


        private const float DisableAlphaValue = .3f;

        public void ShowElement()
        {
            gameObject.SetActive(true);
            alphaGroup.alpha = 1;
        }

        public void DisableElement()
        {
            alphaGroup.alpha = DisableAlphaValue;
        }

        public void HideElement()
        {
            gameObject.SetActive(false);
        }



        public void EntityInjection(CombatEntity entity)
        {
            _user = entity;
            if (entity == null)
            {
                UpdateInfoAsNull();
            }
            else
            {
                UpdateEntityName(entity);
            }

            TickTempo(in TempoTickValues.ZeroValues);
        }

        public void Injection(Sprite roleIcon)
        {
            this.roleIcon.sprite = roleIcon;
        }


        private const string OnNullName = "-------";
        private void UpdateInfoAsNull()
        {
            UpdateEntityName(OnNullName);
        }

        private void UpdateEntityName(CombatEntity entity)
        {
            UpdateEntityName(entity.CombatCharacterName);
        }

        private void UpdateEntityName(string text)
        {
            entityName.text = text;
        }


        public void OnPreStartCombat()
        {

        }

        public void OnInstantiation()
        {
            ShowElement();
        }

        public void OnDestruction()
        {
        }

        public void TickTempo(in TempoTickValues values)
        {
            var currentTickInitiative = values.CurrentTick;
            currentTick.text = currentTickInitiative.ToString("00");

            var steps = values.RemainingSteps;
            remainingStepsText.text = steps.ToString("00");

            entitySpeed.text = HandleSpeedText();

            string HandleSpeedText()
            {
                var userStats = _user.Stats;
                float initiativeSpeed = UtilsStatsFormula.CalculateInitiativeSpeed(userStats);
                return "+" + initiativeSpeed.ToString("##");
            }
        }
    }
}
