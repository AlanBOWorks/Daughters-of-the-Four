using System;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using DG.Tweening;
using Localization.Characters;
using MPUIKIT;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem.Player.UI
{
    public class UTempoTrackerHolder : MonoBehaviour, IEntityExistenceElement<UTempoTrackerHolder>
    {
        [Title("Images")] 
        [SerializeField] private MPImage backgroundImage;
        [SerializeField] private Image backgroundIcon;

        [Title("Texts")]
        [SerializeField] private TextMeshProUGUI entityName;
        [SerializeField] private TextMeshProUGUI currentTick;
        [SerializeField] private TextMeshProUGUI entitySpeed;
        [SerializeField] private TextMeshProUGUI remainingStepsText;

        [SerializeField] private Image roleIcon;
        private Color _roleColor;
        
        [ShowInInspector,DisableInEditorMode]
        private CombatEntity _user;

      
        public void ShowElement()
        {
            gameObject.SetActive(true);
        }
        public void HideElement()
        {
            gameObject.SetActive(false);
        }

        public Image GetBackgroundHolder() => backgroundImage;
        public Image GetBackgroundIconHolder() => backgroundIcon;
        public TextMeshProUGUI GetStepTextHolder() => remainingStepsText;


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

        public void Injection(Sprite roleSprite)
        {
            this.roleIcon.sprite = roleSprite;
        }

        public void Injection(Color roleColor)
        {
            _roleColor = roleColor;
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

        private const int CloseStepThreshold = 1;
        public void TickTempo(in TempoTickValues values)
        {
            var currentTickInitiative = values.CurrentTick;
            currentTick.text = currentTickInitiative.ToString("00");

            var steps = values.RemainingSteps;
            UpdateStepsText(steps);

            entitySpeed.text = HandleSpeedText();

            string HandleSpeedText()
            {
                var userStats = _user.Stats;
                float initiativeSpeed = UtilsStatsFormula.CalculateInitiativeSpeed(userStats);
                return "+" + initiativeSpeed.ToString("##");
            }

            if(steps > CloseStepThreshold) return;

            OnControlClose();
        }

        public void UpdateToCurrent()
        {
            TickTempo(new TempoTickValues(_user));
        }

        public void UpdateStepsText(int steps)
        {
            remainingStepsText.text = steps.ToString("00");
        }

        public void OnControlClose()
        {
            var onCloseColor = new Color(.1f,.1f,.1f);
            backgroundImage.color = onCloseColor;

            remainingStepsText.color = Color.white;
        }

        private const float ColorFadeDuration = .2f;
        public void OnControlStart()
        {
            DOTween.Kill(backgroundIcon);
            Color targetColor = _roleColor;
            targetColor.a = .3f;
            backgroundIcon.DOColor(targetColor,ColorFadeDuration);
        }

        public void OnSequenceFinish(
            in Color initialBackgroundColor,
            in Color initialMainColor,
            in Color iconInitialColor)
        {
            DOTween.Kill(backgroundImage);
            DOTween.Kill(backgroundIcon);
            backgroundImage.DOColor(initialBackgroundColor, ColorFadeDuration);
            remainingStepsText.color = initialMainColor;
            backgroundIcon.color = iconInitialColor;
        }
    }
}
