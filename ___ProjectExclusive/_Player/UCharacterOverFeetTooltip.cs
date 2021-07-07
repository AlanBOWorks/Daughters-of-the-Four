using System;
using _CombatSystem;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Player
{
    public class UCharacterOverFeetTooltip : UCharacterOverTooltipBase
    {
        [TitleGroup("Filler")]
        [SerializeField]
        private TempoFillerTooltip fillerTooltip = new TempoFillerTooltip();

        private void Start()
        {
            fillerTooltip.CalculateStep();
        }

        public override void Injection(CombatingEntity entity)
        {
            base.Injection(entity);
            CombatSystemSingleton.TempoHandler.EntitiesBar.Add(entity, fillerTooltip);
        }

    }

    [Serializable]
    internal class TempoFillerTooltip : ITempoFiller
    {
        [SerializeField] private RectTransform fillerTransform = null;
        private float _barWidth;

        public void CalculateStep()
        {
            _barWidth = fillerTransform.rect.width;
        }


        public void FillBar(float percentage)
        {
            Vector2 reposition = fillerTransform.anchoredPosition;
            reposition.x = Mathf.Lerp(0,_barWidth,percentage);
            fillerTransform.anchoredPosition = reposition;
        }
    }
}
