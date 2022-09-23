using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using ExplorationSystem;
using UnityEngine;

namespace _Injectors.ExplorationCombat
{
    public class UExplorationCombatToggle : MonoBehaviour, IExplorationOnCombatListener, ICombatTerminationListener,
        ICombatPreparationListener
    {
        [SerializeField] private GameObject[] onCombatStartHide;
        [SerializeField] private GameObject[] onExplorationHide;


        private void Awake()
        {
            ExplorationSingleton.EventsHolder.Subscribe(this);
            CombatSystemSingleton.EventsHolder.Subscribe(this);
        }

        private void OnDestroy()
        {
            ExplorationSingleton.EventsHolder.UnSubscribe(this);
            CombatSystemSingleton.EventsHolder.UnSubscribe(this);

        }


        private static void ToggleObjects(IEnumerable<GameObject> elements, bool active)
        {
            foreach (var element in elements)
            {
                element.SetActive(active);
            }
        }

        public void OnExplorationCombatLoadFinish(EnumExploration.ExplorationType type)
        {
            ToggleObjects(onExplorationHide,false);
        }

        public void OnExplorationReturnFromCombat(EnumExploration.ExplorationType fromCombatType)
        {
            ToggleObjects(onExplorationHide,true);
        }

        public void OnCombatFinish(UtilsCombatFinish.FinishType finishType)
        {
        }

        public void OnCombatFinishHide(UtilsCombatFinish.FinishType finishType)
        {
            ToggleObjects(onCombatStartHide,false);
        }

        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            ToggleObjects(onCombatStartHide,true);
        }
    }
}
