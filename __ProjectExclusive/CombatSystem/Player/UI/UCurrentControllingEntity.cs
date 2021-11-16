using CombatEntity;
using CombatSkills;
using CombatSystem.Events;
using TMPro;
using UnityEngine;

namespace __ProjectExclusive.Player.UI
{
    public class UCurrentControllingEntity : MonoBehaviour, ITempoListener<CombatingEntity>, ISkillEventListener
    {
        private void Awake()
        {
            var eventsHolder = PlayerCombatSingleton.PlayerEvents;
            eventsHolder.Subscribe(this as ITempoListener<CombatingEntity>);
            eventsHolder.Subscribe(this);
        }

        [SerializeField] 
        private TextMeshProUGUI currentActionsText;

        private CombatingEntity _currentEntity;
        private void UpdateCurrentActionsAmount(CombatingEntity entity)
        {
            var currentActions = entity.CombatStats.CurrentActions;
            string actionsText = UtilsText.TryGetSingleDigit(currentActions);
            currentActionsText.text = actionsText;
        }

        public void OnFirstAction(CombatingEntity element)
        {
            _currentEntity = element;
            UpdateCurrentActionsAmount(element);
        }

        public void OnFinishAction(CombatingEntity element)
        {
            UpdateCurrentActionsAmount(element);
        }

        public void OnFinishAllActions(CombatingEntity element)
        {
        }

        public void OnSkillUse(SkillValuesHolders values)
        {
            UpdateCurrentActionsAmount(_currentEntity);
        }

        public void OnSkillCostIncreases(SkillValuesHolders values)
        {
            
        }
    }


}
