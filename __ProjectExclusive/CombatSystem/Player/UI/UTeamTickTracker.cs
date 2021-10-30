using System.Collections.Generic;
using CombatEntity;
using CombatSystem;
using CombatTeam;
using TMPro;
using UnityEngine;

namespace __ProjectExclusive.Player
{
    public class UTeamTickTracker : UPersistentGroupStructureBase<UTickTracker>, IEntityTickListener
    {

        protected override void Awake()
        {
            base.Awake();
            _trackers = new Dictionary<CombatingEntity, UTickTracker>();
            CombatSystemSingleton.TempoTicker.Subscribe(this);
        }

        private Dictionary<CombatingEntity, UTickTracker> _trackers;
        [SerializeField] private TextMeshProUGUI tickThresholdText;

        protected override void DoInjection(CombatingEntity entity, UTickTracker element)
        {
            if(entity == null || element == null) return;

            _trackers.Add(entity,element);
        }

        public override void OnAfterLoads()
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
            tickThresholdText.SetText(initiativeCheckAmount.ToString("00"));
        }

        public void OnTickEntity(CombatingEntity entity, float currentTickAmount)
        {
            _trackers[entity].UpdateTickCount(currentTickAmount);
        }

    }


}
