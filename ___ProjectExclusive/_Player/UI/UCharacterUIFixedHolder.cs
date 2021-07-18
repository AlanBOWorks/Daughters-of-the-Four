﻿using System;
using System.Globalization;
using ___ProjectExclusive;
using _CombatSystem;
using _Player;
using Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Player
{
    public class UCharacterUIFixedHolder : UCharacterUIHolderBase
    {
        [SerializeField] private TextMeshProUGUI characterName = null;
        //TODO give Injection
        [SerializeField] private TempoFillerTooltip tempoFiller = new TempoFillerTooltip();
        public override void Injection(CombatingEntity entity)
        {
            characterName.text = entity.CharacterName;
            base.Injection(entity);
            tempoFiller.Injection(entity);
        }
    }

    public abstract class UCharacterUIHolderBase : MonoBehaviour
    {
        [SerializeField] private HealthTooltip healthTooltip = new HealthTooltip();

        public virtual void Injection(CombatingEntity entity)
        {
            healthTooltip.Injection(entity);
        }
    }

    [Serializable]
    internal class HealthTooltip : IVitalityChangeListener, ITemporalStatsChangeListener
    {
        [SerializeField] private TextMeshProUGUI healthAmount = null;
        [SerializeField] private TextMeshProUGUI maxHealth = null;

        public void Injection(CombatingEntity entity)
        {
            var stats = entity.CombatStats;

            OnVitalityChange(stats);
            OnTemporalStatsChange(stats);

            entity.Events.SubscribeListener(this);
        }


        public void OnVitalityChange(IVitalityStats currentStats)
        {

            maxHealth.text = UtilsGameTheme.GetNumericalPrint(currentStats.MaxHealth);
        }

        public void OnTemporalStatsChange(ICombatTemporalStats currentStats)
        {
            healthAmount.text = UtilsGameTheme.GetNumericalPrint(currentStats.HealthPoints);
        }
    }

    //This isn't a ICharacterListener since requires it update every deltaTime
    [Serializable]
    internal class TempoFillerTooltip : ITempoFiller
    {
        [SerializeField] private RectTransform fillerTransform = null;
        private float _barWidth;

        private void CalculateStep()
        {
            _barWidth = fillerTransform.rect.width;
        }


        public void FillBar(float percentage)
        {
            Vector2 reposition = fillerTransform.anchoredPosition;
            reposition.x = Mathf.Lerp(0, _barWidth, percentage);
            fillerTransform.anchoredPosition = reposition;
        }

        public virtual void Injection(CombatingEntity entity)
        {
            CombatSystemSingleton.TempoHandler.EntitiesBar.Add(entity, this);
            CalculateStep();
        }
    }
}
