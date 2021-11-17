using CombatEntity;
using DG.Tweening;
using Stats;
using TMPro;
using UnityEngine;

namespace __ProjectExclusive.Player.UI
{
    public class UActionsStateUIHolder : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI actionsText;
        private IConcentrationStatsRead<float> _stats;

        public void Inject(CombatingEntity entity)
        {
            _stats = entity.CombatStats;
            UpdateActions((int)_stats.ActionsPerSequence);
        }

        public void AnimateCurrentActions() => UpdateActionsAnimated((int)_stats.ActionsPerSequence);

        private void UpdateActions(int actionsAmount)
        {
            string digitText = UtilsText.GetSingleDigit(actionsAmount);
            actionsText.text = digitText;
        }

        private void UpdateActionsAnimated(int actionsAmount)
        {
            UpdateActions(actionsAmount);
            var actionTextTransform = actionsText.rectTransform;
            actionTextTransform.DOPunchPosition(Vector3.up, .2f);
        }
    }
}
