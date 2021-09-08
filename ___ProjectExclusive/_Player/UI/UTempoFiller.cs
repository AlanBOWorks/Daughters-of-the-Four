using System;
using _CombatSystem;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace _Player
{
    public class UTempoFiller : MonoBehaviour, ITempoFiller
    {
        [Title("Params")] 
        [SerializeField]
        private EnumCharacter.RoleArchetype role = EnumCharacter.RoleArchetype.Vanguard;
        [SerializeField] 
        private bool isPlayerFiller = true;

        [Title("UI")]
        [SerializeField] 
        private RectTransform fillerTransform = null;
        private float _barWidth;

        private void Awake()
        {
            CalculateStep();
            InjectInTempoHandler();
        }

        private void CalculateStep()
        {
            _barWidth = fillerTransform.rect.width;
        }

        private void InjectInTempoHandler()
        {
            var tempoHandler = CombatSystemSingleton.TempoHandler;
            var fillerHolder = tempoHandler.GetFillerHolder(role, isPlayerFiller);
            fillerHolder.Add(this);
        }

        public void FillBar(float percentage)
        {
            Vector2 reposition = fillerTransform.anchoredPosition;
            reposition.x = Mathf.Lerp(0, _barWidth, percentage);
            fillerTransform.anchoredPosition = reposition;
        }
    }
}
