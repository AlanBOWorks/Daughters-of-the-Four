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
        
        private void Start()
        {
            CombatSystemSingleton.Invoker.SubscribeListener(this);
        }

        public void OnAfterPreparation(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
        {
            var rectTransform = (RectTransform)transform;
            _lineWidthHalf = rectTransform.rect.width * .5f; 
            breakPoints.DoInitialPosition(_lineWidthHalf,playerEntities,enemyEntities);
            breakPoints.Show(true);

            CombatSystemSingleton.CombatTeamControlHandler.Subscribe(this);
        }

        public void OnPlayerControlVariation(float controlPercentage, TeamCombatState.Stance stance)
        {
            OnPlayerControlVariation(controlPercentage);
            stateTooltip.text = UtilsTeam.GetKeyword(stance);
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
            [SerializeField] private Points enemyPoints;
            [SerializeField] private Points playerPoints;

            internal void DoInitialPosition(float halfWidth, CombatingTeam playerEntities, CombatingTeam enemyEntities)
            {
                // Negative modifier because threshold is measured in negative values
                // (and player goes to the left)

                enemyPoints.DoPosition(halfWidth,
                    -enemyEntities.StatsHolder.LoseControlThreshold);
                playerPoints.DoPosition(halfWidth,
                    playerEntities.StatsHolder.LoseControlThreshold); 
            }

            internal void Show(bool show)
            {
                enemyPoints.Show(show);
                playerPoints.Show(show);
            }

            [Serializable]
            struct Points
            {
                [SerializeField] private RectTransform breakPoint;

                internal void DoPosition(float halfWith, float modifier)
                {
                    Vector2 linePosition = Vector2.zero;
                    linePosition.x = halfWith * modifier;
                    breakPoint.anchoredPosition = linePosition;
                }

                internal void Show(bool show)
                {
                    breakPoint.gameObject.SetActive(show);
                }
            }
        }
    }


}
