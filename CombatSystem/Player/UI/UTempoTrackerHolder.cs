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

        [SerializeField] private TextMeshProUGUI entityName;
        [SerializeField] private TextMeshProUGUI currentTick;
        [SerializeField] private TextMeshProUGUI entitySpeed;
        [SerializeField] private RectTransform percentBarImage;

        [ShowInInspector]
        private CombatEntity _user;

        public const float HeightElementSeparation = 16* 2 + 10;
        private float _barInitialWidth;
        private void Awake()
        {
            _barInitialWidth = percentBarImage.rect.width;
        }

        public void ShowElement()
        {
            gameObject.SetActive(true);
        }

        public void HideElement()
        {
            gameObject.SetActive(false);
        }

        public void EntityInjection(in CombatEntity entity)
        {
            _user = entity;

            UpdateEntityName(in entity);
            TickTempo(0,0);
        }


        private void UpdateEntityName(in CombatEntity entity)
        {
            entityName.text = entity.CombatCharacterName;
        }

        public void RepositionLocalHeight(int index)
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
                targetSize => percentBarImage.sizeDelta = targetSize,
                desiredSize,
                AnimationDuration);
        }

        private void CalculateBarWidth(in float percentInitiative, out Vector2 barSize, out Vector2 desiredSize)
        {
            float inversePercent =  1- percentInitiative;
            float invertInitialWidth = -_barInitialWidth; // negative because negative sizeDelta means smaller

            float targetWidth = inversePercent * invertInitialWidth;
            targetWidth = Mathf.Clamp(targetWidth,invertInitialWidth, 0); // Zero means full holder's rect.deltaSize

            barSize = percentBarImage.sizeDelta;
            desiredSize = new Vector2(targetWidth, barSize.y);
        }

        private void UpdateSpeedText()
        {
            if(_user == null) return;

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
