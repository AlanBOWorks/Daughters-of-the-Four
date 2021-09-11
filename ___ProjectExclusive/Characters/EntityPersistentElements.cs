using System;
using System.Collections.Generic;
using _CombatSystem;
using _Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Characters
{
    public class EntityPersistentElements : IPersistentEntitySwitchListener
    {
        public EntityPersistentElements()
        {
            TempoFillers = new List<ITempoFiller>();
            CombatEvents = new CombatCharacterEventsBase();

            _entitySwitchListeners = new Queue<IPersistentEntitySwitchListener>();
        }

        [ShowInInspector]
        public readonly List<ITempoFiller> TempoFillers;
        [ShowInInspector]
        public readonly CombatCharacterEventsBase CombatEvents;

        [ShowInInspector]
        private readonly Queue<IPersistentEntitySwitchListener> _entitySwitchListeners;
        public void SubscribeListener(IPersistentEntitySwitchListener listener)
            => _entitySwitchListeners.Enqueue(listener);

        public void OnEntitySwitch(CombatingEntity entity)
        {
            foreach (IPersistentEntitySwitchListener listener in _entitySwitchListeners)
            {
                listener.OnEntitySwitch(entity);
            }
        }
    }

    public class PersistentElementsDictionary : Dictionary<CombatingEntity,EntityPersistentElements>,
        ITeamsData<ICharacterArchetypesData<EntityPersistentElements>>,
        ICombatPreparationListener
    {
        public PersistentElementsDictionary()
        {
            PlayerData = new TeamElements();
            EnemyData = new TeamElements();
        }
        [ShowInInspector]
        public ICharacterArchetypesData<EntityPersistentElements> PlayerData { get; }
        [ShowInInspector]
        public ICharacterArchetypesData<EntityPersistentElements> EnemyData { get; }


        public void DoInjectionIn(IPersistentInjectorHolders injection)
        {
            var playerInjectors = injection.GetPlayerInjectors();
            var enemyInjectors = injection.GetEnemyInjectors();

            DoInjection(PlayerData,playerInjectors);
            DoInjection(EnemyData,enemyInjectors);

            void DoInjection(
                ICharacterArchetypesData<EntityPersistentElements> elements,
                ICharacterArchetypesData<IPersistentElementInjector> injectors)
            {
                injectors.Vanguard.DoInjection(elements.Vanguard);
                injectors.Attacker.DoInjection(elements.Attacker);
                injectors.Support.DoInjection(elements.Support);
            }
        }

        public EntityPersistentElements GetElement(EnumCharacter.RoleArchetype role, bool isPlayer)
        {
            var dataHolder = (isPlayer) ? PlayerData : EnemyData;
            return UtilsCharacter.GetElement(dataHolder, role);
        }

        public void OnBeforeStart(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
        {
            Clear();
            UtilsCharacterArchetypes.InjectInDictionary(this, playerEntities, PlayerData);
            UtilsCharacterArchetypes.InjectInDictionary(this, enemyEntities, EnemyData);

            foreach (var pair in this)
            {
                pair.Value.OnEntitySwitch(pair.Key);
            }
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

    public interface IPersistentInjectorHolders
    {
        ICharacterArchetypesData<IPersistentElementInjector> GetPlayerInjectors();
        ICharacterArchetypesData<IPersistentElementInjector> GetEnemyInjectors();
    }

    public interface IPersistentElementInjector
    {
        void DoInjection(EntityPersistentElements persistentElements);
    }

    public interface IPersistentEntitySwitchListener
    {
        void OnEntitySwitch(CombatingEntity entity);
    }
}
