using System;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using DG.Tweening;
using Localization.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem.Player.UI
{
    public class UTempoTrackerHolder : MonoBehaviour, IEntityExistenceElement<UTempoTrackerHolder>
    {

        [SerializeField] private TextMeshProUGUI entityName;
        [SerializeField] private TextMeshProUGUI currentTick;
        [SerializeField] private TextMeshProUGUI entitySpeed;
        [SerializeField] private Image percentBar;
        private RectTransform _percentBarTransform;

        
        public const float HeightElementSeparation = 16* 2 + 10;
        private float _barInitialWidth;
        private void AllocateBar()
        {
            _percentBarTransform = percentBar.rectTransform;
            _barInitialWidth = _percentBarTransform.rect.width;
        }

        private CombatEntity _user;
        public void EntityInjection(in CombatEntity entity, int index)
        {
            if(!_percentBarTransform) AllocateBar();

            _user = entity;

            UpdateEntityName(in entity);
            RepositionLocalHeight(in index);
            TickTempo(0,0);

            gameObject.SetActive(true);
        }

        private void UpdateEntityName(in CombatEntity entity)
        {
            entityName.text = LocalizationPlayerCharacters.LocalizeCharactersName(entity);
        }
        private void RepositionLocalHeight(in int index)
        {
            var rectTransform = GetComponent<RectTransform>();
            const float transformHeight = HeightElementSeparation;

            Vector3 localPosition = rectTransform.localPosition;
            localPosition.y = -transformHeight * index;
            rectTransform.localPosition = localPosition;
        }

        public void OnPreStartCombat()
        {

        }

        private const float AnimationTimeOffset = .02f;
        private const float AnimationDuration = TempoTicker.TickPeriodSeconds - AnimationTimeOffset;
        public void TickTempo(in float currentInitiative, in float percentInitiative)
        {
            UpdateCurrentTick(in currentInitiative);
            UpdateSpeedText();

            CalculateBarWidth(percentInitiative, out Vector2 barSize, out Vector2 desiredSize);
            AnimateBar(barSize,  desiredSize);
        }

        private void AnimateBar(Vector2 barSize, Vector2 desiredSize)
        {
            DOTween.To(
                () => barSize,
                targetSize => _percentBarTransform.sizeDelta = targetSize,
                desiredSize,
                AnimationDuration);
        }

        private void CalculateBarWidth(in float percentInitiative, out Vector2 barSize, out Vector2 desiredSize)
        {
            float targetWidth = _barInitialWidth * (percentInitiative);
            if (targetWidth > _barInitialWidth) targetWidth = _barInitialWidth;
            else if (targetWidth < 0) targetWidth = 0;

            barSize = _percentBarTransform.sizeDelta;
            desiredSize = new Vector2(targetWidth, barSize.y);
        }

        private void UpdateSpeedText()
        {
            var userStats = _user.Stats;
            float initiativeSpeed = UtilsStatsFormula.CalculateInitiativeSpeed(in userStats);
            entitySpeed.text = "+" + initiativeSpeed.ToString("0");
        }

        private void UpdateCurrentTick(in float currentInitiative)
        {
            currentTick.text = currentInitiative.ToString("00");
        }
    }
}
