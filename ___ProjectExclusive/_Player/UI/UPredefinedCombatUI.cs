using System;
using System.Collections.Generic;
using _CombatSystem;
using _Team;
using Characters;
using UnityEngine;

namespace _Player
{
    public class UPredefinedCombatUI : MonoBehaviour, ICombatAfterPreparationListener
    {
        [SerializeField] FactionsFixedUI FixedUi = new FactionsFixedUI();
        private PredefinedUIHolderDictionary _characterFixedHolders;
        private void Awake()
        {
            CombatSystemSingleton.Invoker.SubscribeListener(this);
            _characterFixedHolders = new PredefinedUIHolderDictionary();
            PlayerEntitySingleton.PredefinedUIDictionary = _characterFixedHolders;
        }


        public void OnAfterPreparation(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
        {
            _characterFixedHolders.Clear();

            FixedUi.Injection(playerEntities, enemyEntities, _characterFixedHolders);
        }

    }

    public class PredefinedUIHolderDictionary : Dictionary<CombatingEntity, UCharacterUIFixedHolder>
    {}

    [Serializable]
    internal class FactionsFixedUI : ICharacterFaction<TeamCombatFixedUI>
    {
        [SerializeField] private TeamCombatFixedUI playerFaction = new TeamCombatFixedUI();
        [SerializeField] private TeamCombatFixedUI enemyFaction = new TeamCombatFixedUI();

        public TeamCombatFixedUI PlayerFaction => playerFaction;
        public TeamCombatFixedUI EnemyFaction => enemyFaction;
        public void Injection(CombatingTeam playerEntities, CombatingTeam enemyEntities,
            PredefinedUIHolderDictionary holders)
        {
            playerFaction.DoInjection(playerEntities,holders);
            enemyFaction.DoInjection(enemyEntities,holders);
        }
    }

    [Serializable]
    internal class TeamCombatFixedUI : SerializablePlayerArchetypes<UCharacterUIFixedHolder>
    {
        public void DoInjection(CombatingTeam team, PredefinedUIHolderDictionary dictionary)
        {
#if UNITY_EDITOR
            if (team.Count != 3)
            {
                throw new NotImplementedException("UI elements can't support != 3 elements");
            }
#endif
            DoInjection(team.FrontLiner,FrontLiner);
            DoInjection(team.MidLiner,MidLiner);
            DoInjection(team.BackLiner,BackLiner);

            void DoInjection(CombatingEntity entity, UCharacterUIFixedHolder holder)
            {
                holder.Injection(entity);
                dictionary.Add(entity,holder);
            }
        }
    }
}
