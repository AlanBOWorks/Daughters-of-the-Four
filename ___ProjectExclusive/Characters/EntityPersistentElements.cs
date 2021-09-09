using System;
using System.Collections.Generic;
using _CombatSystem;
using _Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Characters
{
    public class EntityPersistentElements
    {
        public EntityPersistentElements()
        {
            TempoFillers = new List<ITempoFiller>();
            CombatEvents = new CombatCharacterEventsBase();
        }

        [ShowInInspector]
        public readonly List<ITempoFiller> TempoFillers;
        [ShowInInspector]
        public readonly CombatCharacterEventsBase CombatEvents;
    }

    public class TeamsPersistentElements : Dictionary<CombatingEntity,EntityPersistentElements>,
        ITeamsData<ICharacterArchetypesData<EntityPersistentElements>>,
        ICombatPreparationListener
    {
        public TeamsPersistentElements()
        {
            PlayerData = new TeamElements();
            EnemyData = new TeamElements();
        }
        [ShowInInspector]
        public ICharacterArchetypesData<EntityPersistentElements> PlayerData { get; }
        [ShowInInspector]
        public ICharacterArchetypesData<EntityPersistentElements> EnemyData { get; }

        public EntityPersistentElements GetElement(EnumCharacter.RoleArchetype role, bool isPlayer)
        {
            var dataHolder = (isPlayer) ? PlayerData : EnemyData;
            return UtilsCharacter.GetElement(dataHolder, role);
        }

        private class TeamElements : CharacterArchetypes<EntityPersistentElements>
        {
            public TeamElements()
            {
                Vanguard = new EntityPersistentElements();
                Attacker = new EntityPersistentElements();
                Support = new EntityPersistentElements();
            }
        }

        public void OnBeforeStart(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
        {
            Clear();
            UtilsCharacterArchetypes.InjectInDictionary(this, playerEntities, PlayerData);
            UtilsCharacterArchetypes.InjectInDictionary(this, enemyEntities, EnemyData);
        }
    }


    public abstract class UPersistentElementInjector : MonoBehaviour
    {
        [SerializeField] private Parameters parameters = new Parameters();

        private void Start()
        {
            var persistentElements = parameters.GetElement();
            DoInjection(persistentElements);
            parameters = null;
        }

        protected abstract void DoInjection(EntityPersistentElements persistentElements);

        [Serializable]
        private class Parameters
        {
            [SerializeField] private EnumCharacter.RoleArchetype role 
                = EnumCharacter.RoleArchetype.Vanguard;

            [SerializeField] private bool isPlayerEntity = true;

            public EntityPersistentElements GetElement()
                => CombatSystemSingleton.TeamsPersistentElements.GetElement(role, isPlayerEntity);
        }
    }
}
