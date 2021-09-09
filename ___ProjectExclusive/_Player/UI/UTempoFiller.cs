using System;
using _CombatSystem;
using Characters;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace _Player
{
    public class UTempoFiller : UPersistentElementInjector, ITempoFiller
    {
        [Title("UI")]
        [SerializeField] 
        private RectTransform fillerTransform = null;
        private float _barWidth;

        private void Awake()
        {
            CalculateStep();
        }

        private void CalculateStep()
        {
            _barWidth = fillerTransform.rect.width;
        }

        public void FillBar(float percentage)
        {
            float xPosition = Mathf.Lerp(0, _barWidth, percentage);
            
            fillerTransform.DOAnchorPosX(xPosition, TempoHandler.DeltaStepPeriod);
        }

        protected override void DoInjection(EntityPersistentElements persistentElements)
        {
            persistentElements.TempoFillers.Add(this);
        }
    }
}
