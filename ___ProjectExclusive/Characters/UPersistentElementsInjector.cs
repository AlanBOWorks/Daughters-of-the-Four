using System;
using _CombatSystem;
using _Team;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Characters
{
    public abstract class UPersistentElementsInjector<T> : MonoBehaviour where T: IPersistentElementInjector, new()
    {
        [Title("Params")] 
        [SerializeField] private bool isPlayerInjection = true;
        [Title("Elements")]
        [SerializeField] private ElementsHolder elements = new ElementsHolder();


        private void Awake()
        {
            PersistentElementsDictionary elementsHolder = CombatSystemSingleton.TeamsPersistentElements;
            var teamElements = elementsHolder.GetTeamElements(isPlayerInjection);

            UtilsCharacter.DoInjection(elements,teamElements);
            Destroy(this);
        }

        [Serializable]
        private class ElementsHolder : SerializableCharacterArchetypes<T>
        {
            
        }
    }

    public abstract class UMonoPersistentElementInjector<T> : MonoBehaviour, ICharacterArchetypesData<T> where T : Object, IPersistentElementInjector
    {
        [Title("Params")]
        [SerializeField] private bool isPlayerInjection = true;
        [Title("Elements")]
        [SerializeField] private T vanguard;
        [SerializeField] private T attacker;
        [SerializeField] private T support;

        public T Vanguard => vanguard;
        public T Attacker => attacker;
        public T Support => support;

        private void Awake()
        {
            PersistentElementsDictionary elementsHolder = CombatSystemSingleton.TeamsPersistentElements;
            var teamElements = elementsHolder.GetTeamElements(isPlayerInjection);

            UtilsCharacter.DoInjection(this, teamElements);
            Destroy(this);
        }

    }
}
