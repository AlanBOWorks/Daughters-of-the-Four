using System.Collections.Generic;
using CombatEntity;
using CombatSystem;
using CombatTeam;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace __ProjectExclusive.Player.UI
{
    public class UTeamTickTracker : UPersistentGroupStructureBase<UTickTracker>, IEntityTickListener
    {

        protected override void Awake()
        {
            base.Awake();
            _trackers = new Dictionary<CombatingEntity, UTickTracker>();
            CombatSystemSingleton.CombatingEntitiesTicker.EntityTickListeners.Add(this);
        }

        private Dictionary<CombatingEntity, UTickTracker> _trackers;
        [SerializeField, HorizontalGroup("Round ticks")] 
        private TextMeshProUGUI roundTickThresholdText;
        [SerializeField, HorizontalGroup("Round ticks")] 
        private TextMeshProUGUI currentRoundTickAmount;

        protected override void DoInjection(CombatingEntity entity, UTickTracker element)
        {
            if(entity == null || element == null) return;

            _trackers.Add(entity,element);
        }

        public override void OnAfterLoadsCombat()
        {
        }

        public override void OnCombatPause()
        {
            
        }

        public override void OnCombatResume()
        {
            
        }

        public override void OnCombatExit()
        {
            _trackers.Clear();
        }

        public void TickThresholdInjection(float initiativeCheckAmount)
        {
        }

        public void OnTickEntity(CombatingEntity entity, float currentTickAmount)
        {
            _trackers[entity].UpdateTickCount(currentTickAmount);
        }

        public void RoundAmountInjection(int triggerAmount)
        {
            var digitText = UtilsText.TryGetRoundDigit(triggerAmount);
            roundTickThresholdText.SetText(digitText);
        }

        public void OnRoundTick(int currentTickCount)
        {
            var digitText = UtilsText.TryGetRoundDigit(currentTickCount);
            currentRoundTickAmount.SetText(digitText);
        }
    }


}
