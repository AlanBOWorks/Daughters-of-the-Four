using System;
using _CombatSystem;
using _Team;
using Characters;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Player
{
    public class UControlTracker : MonoBehaviour, ITeamVariationListener
    {
        [SerializeField] private RectTransform percentageIcon;
        [SerializeField] private TextMeshProUGUI stateTooltip;
        [SerializeField] private BreakPoints breakPoints = new BreakPoints();
        private float _lineWidthHalf;

        private void Awake()
        {
            var rectTransform = (RectTransform) transform;
            _lineWidthHalf = rectTransform.rect.width *.5f;

            breakPoints.DoInitialPosition(_lineWidthHalf);
        }

        private void Start()
        {
            CombatSystemSingleton.Invoker.SubscribeListener(this);
        }

        public void OnAfterPreparation(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
        {
            CombatSystemSingleton.TeamsDataHandler.Subscribe(this);
        }

        public void OnPlayerControlVariation(float controlPercentage, TeamCombatData.Stance stance)
        {
            OnPlayerControlVariation(controlPercentage);
            stateTooltip.text = UtilsTeam.GetKeyword(stance);

            bool showExtremes = stance == TeamCombatData.Stance.Neutral;
            breakPoints.ShowExtreme(showExtremes);
        }

        public void OnPlayerControlVariation(float controlPercentage)
        {
            Vector2 barPosition = percentageIcon.anchoredPosition;
            barPosition.x = _lineWidthHalf * controlPercentage;
            percentageIcon.anchoredPosition = barPosition;
        }


        [Serializable]
        private class BreakPoints
        {
            [SerializeField] private Points negatives;
            [SerializeField] private Points positives;

            internal void DoInitialPosition(float halfWidth)
            {
                negatives.DoPosition(halfWidth,-1);
                positives.DoPosition(halfWidth,1);
            }

            internal void ShowExtreme(bool show)
            {
                negatives.ShowExtreme(show);
                positives.ShowExtreme(show);
            }

            [Serializable]
            struct Points
            {
                [SerializeField] private RectTransform neutralReturnPoint;
                [SerializeField] private RectTransform neutralBreakPoint;

                internal void DoPosition(float halfWith, float modifier)
                {
                    Vector2 linePosition = Vector2.zero;
                    linePosition.x = halfWith * CombatTeamControlsHandler.NeutralReturnModifier * modifier;
                    neutralReturnPoint.anchoredPosition = linePosition;
                    linePosition.x = halfWith * CombatTeamControlsHandler.NeutralBreakModifier * modifier;
                    neutralBreakPoint.anchoredPosition = linePosition;
                }

                internal void ShowExtreme(bool show)
                {
                    neutralReturnPoint.gameObject.SetActive(!show);
                    neutralBreakPoint.gameObject.SetActive(show);
                }
            }
        }
    }


}
