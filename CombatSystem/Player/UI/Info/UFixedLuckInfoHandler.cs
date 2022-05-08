using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UFixedLuckInfoHandler : MonoBehaviour, 
        IEntityElementInstantiationListener<UVitalityInfo>, //This is because I want to use UDualMainVitalityInfosHandler's events
        ITempoEntityStatesListener,
        ICombatStatesListener
    {
        [ShowInInspector,DisableInEditorMode]
        private Dictionary<CombatEntity, ULuckInfo> _elementsDictionary;
        private void Start()
        {
            _elementsDictionary = new Dictionary<CombatEntity, ULuckInfo>();
            CombatSystemSingleton.EventsHolder.Subscribe(this);
        }

        private void ResetState()
        {
            _elementsDictionary.Clear();
        }

        public void OnCombatPreStarts()
        {
            ResetState();
        }

        public void OnFinishPreStarts()
        {
            foreach (var pair in _elementsDictionary)
            {
                var element = pair.Value;
                var entity = pair.Key;
                UpdateLuckInfo(in element, in entity);
            }
        }

        public void OnIterationCall(in UVitalityInfo element, in CombatEntity entity, in TeamStructureIterationValues values)
        {
            if(entity == null) return;

            var luckInfoHolder = element.GetComponent<ULuckInfo>();
            _elementsDictionary.Add(entity, luckInfoHolder);
        }
        public void OnEntityRequestSequence(CombatEntity entity, bool canControl)
        {
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
            UpdateLuckInfo(in entity);
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
        }

        public void OnEntityFinishSequence(CombatEntity entity, in bool isForcedByController)
        {
            if(isForcedByController) return;

            UpdateLuckInfo(in entity);
        }

        private void UpdateLuckInfo(in CombatEntity entity)
        {
            if(!_elementsDictionary.ContainsKey(entity)) return;

            var element = _elementsDictionary[entity];
            UpdateLuckInfo(in element, in entity);
        }

        private static void UpdateLuckInfo(in ULuckInfo element, in CombatEntity entity)
        {
            var entityLuck = entity.DiceValuesHolder.CombatPercentageRoll;
            element.UpdateLuck(entityLuck.ToString("P1"));

        }

        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            
        }

        public void OnCombatStart()
        {
        }

        public void OnCombatEnd()
        {
            ResetState();
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
        {
        }
    }
}
