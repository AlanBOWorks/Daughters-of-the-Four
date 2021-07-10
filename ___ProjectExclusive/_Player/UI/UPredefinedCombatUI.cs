using System;
using _CombatSystem;
using Characters;
using UnityEngine;

namespace _Player
{
    public class UPredefinedCombatUI : MonoBehaviour, ICombatAfterPreparationListener
    {
        [SerializeField] FactionsFixedUI FixedUi = new FactionsFixedUI();
        public void OnAfterPreparation(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
        {
            FixedUi.OnBeforeStart(playerEntities,enemyEntities,allEntities);
        }

        private void Awake()
        {
            CombatSystemSingleton.Invoker.SubscribeListener(this);
        }
    }
    [Serializable]
    internal class FactionsFixedUI : ICharacterFaction<TeamCombatFixedUI>
    {
        [SerializeField] private TeamCombatFixedUI playerFaction = new TeamCombatFixedUI();
        [SerializeField] private TeamCombatFixedUI enemyFaction = new TeamCombatFixedUI();

        public TeamCombatFixedUI PlayerFaction => playerFaction;
        public TeamCombatFixedUI EnemyFaction => enemyFaction;
        public void OnBeforeStart(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
        {
            playerFaction.DoInjection(playerEntities);
            enemyFaction.DoInjection(enemyEntities);
        }
    }

    [Serializable]
    internal class TeamCombatFixedUI : SerializablePlayerArchetypes<UCharacterUIFixedHolder>
    {
        public void DoInjection(CombatingTeam team)
        {
#if UNITY_EDITOR
            if (team.Count != 3)
            {
                throw new NotImplementedException("UI elements can't support != 3 elements");
            }
#endif

            FrontLiner.Injection(team.FrontLiner);
            MidLiner.Injection(team.MidLiner);
            BackLiner.Injection(team.BackLiner);
        }
    }
}
