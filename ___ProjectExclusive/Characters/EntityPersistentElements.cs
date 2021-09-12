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

    public sealed class PersistentElementsDictionary : DictionaryEntityElements<EntityPersistentElements>
    {
        protected override EntityPersistentElements GenerateElement()
        => new EntityPersistentElements();

        public ICharacterArchetypesData<EntityPersistentElements> GetTeamElements(bool isPlayer)
            => (isPlayer) ? PlayerData : EnemyData;
    }

    public interface IPersistentElementInjector
    {
        void DoInjection(EntityPersistentElements persistentElements);
    }


    public abstract class UPersistentElementInjector : MonoBehaviour, IPersistentElementInjector
    {
        [SerializeField] private Parameters parameters = new Parameters();

        private void Awake()
        {
            var persistentElements = parameters.GetElement();
            DoInjection(persistentElements);
            parameters = null;
        }

        public abstract void DoInjection(EntityPersistentElements persistentElements);

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
