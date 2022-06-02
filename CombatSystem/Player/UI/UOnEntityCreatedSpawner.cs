using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    /// <summary>
    /// A handler that instantiates the [<see cref="T"/>] for each [<seealso cref="CombatEntity"/>] during the
    /// [<see cref="ICombatEntityExistenceListener"/>]'s events
    /// </summary>
    public abstract class UOnEntityCreatedSpawner<T> : MonoBehaviour, 
        ICombatStatesListener,
        ICombatPreparationListener,
        ICombatEntityExistenceListener
    where T : MonoBehaviour,IEntityExistenceElement<T>
    {
        [SerializeField] private EntityElementSpawner playerElementSpawner;
        [SerializeField] private EntityElementSpawner enemyElementSpawner;
        protected EntityElementSpawner PlayerElementSpawner => playerElementSpawner;
        protected EntityElementSpawner EnemyElementSpawner => enemyElementSpawner;
        
        [ShowInInspector,HideInEditorMode]
        protected Dictionary<CombatEntity, T> ActiveElementsDictionary;

        public IReadOnlyDictionary<CombatEntity, T> GetDictionary() => ActiveElementsDictionary;

        protected virtual void Awake()
        {
            ActiveElementsDictionary = new Dictionary<CombatEntity, T>();
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
        }

        private void Start()
        {
            playerElementSpawner.Disable();
            enemyElementSpawner.Disable();
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }




        public virtual void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            var playerSpawner = PlayerElementSpawner;
            var enemySpawner = EnemyElementSpawner;
            GenerateElements(in playerSpawner, in playerTeam);
            GenerateElements(in enemySpawner, in enemyTeam);

            void GenerateElements(in EntityElementSpawner spawner, in CombatTeam team)
            {
                var members = team.GetAllMembers();
                foreach (var member in members)
                {
                    OnSpawnElementForEntity(in member, in spawner);
                }
            }
        }
        public void OnCreateEntity(in CombatEntity entity, in bool isPlayers)
        {
            var elementSpawner = GetHandler(isPlayers);
            OnSpawnElementForEntity(in entity, in elementSpawner);
        }

        protected virtual void OnSpawnElementForEntity(in CombatEntity entity, in EntityElementSpawner spawner)
        {
            var element = GenerateElement(in entity, spawner);
            OnElementCreated(in element, in entity);
        }
        protected virtual void OnElementCreated(in T element, in CombatEntity entity)
        {
            ActiveElementsDictionary.Add(entity,element);
        }

        protected virtual T GenerateElement(in CombatEntity entity, in EntityElementSpawner spawner)
        {
            return spawner.GenerateElement(in entity);
        }

        public void OnDestroyEntity(in CombatEntity entity, in bool isPlayers)
        {
            var elementSpawner = GetHandler(isPlayers);
            OnDestroyEntity(in entity, elementSpawner);
        }

        private void OnDestroyEntity(in CombatEntity entity, in EntityElementSpawner spawner)
        {
            var element = ActiveElementsDictionary[entity];
            spawner.PushElement(in element);
            ActiveElementsDictionary.Remove(entity);
        }

        private EntityElementSpawner GetHandler(in bool isPlayer) => isPlayer
            ? PlayerElementSpawner
            : EnemyElementSpawner;


        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            foreach (var element in ActiveElementsDictionary)
            {
                element.Value.OnPreStartCombat();
            }
        }

        public virtual void OnCombatStart()
        {
        }

        public virtual void OnCombatEnd()
        {
            foreach (var pair in ActiveElementsDictionary)
            {
                var element = pair.Value;
                var entity = pair.Key;
                bool isPlayerElement = UtilsTeam.IsPlayerTeam(in entity);
                var handler = GetHandler(in isPlayerElement);
                handler.PushElement(in element);
            }
            ActiveElementsDictionary.Clear();
            DisableSpawner(in playerElementSpawner);
            DisableSpawner(in enemyElementSpawner);

            
        }
        private static void DisableSpawner(in EntityElementSpawner spawner)
        {
            spawner.Disable();
        }


        public virtual void OnCombatFinish(bool isPlayerWin)
        {
        }

        public virtual void OnCombatQuit()
        {
            
        }



        [Serializable]
        protected sealed class EntityElementSpawner
        {
            [SerializeField] private Transform instantiationParent;
            [SerializeField] private T elementEntityPrefab;
            [SerializeField] private bool hideElementOnStart = true;
            public int ActiveCount { get; private set; }


            public void Disable()
            {
                if (hideElementOnStart)
                    elementEntityPrefab.gameObject.SetActive(false);
            }

            public T GenerateElement(in CombatEntity entity)
            {
                T element = Instantiate(elementEntityPrefab, instantiationParent);
                element.EntityInjection(entity);
                element.OnInstantiation();
                ActiveCount++;


#if UNITY_EDITOR
                element.name = entity.GetProviderEntityName() + " - UI Hover [Group] (Clone)";
#endif

                return element;
            }

            public void PushElement(in T element)
            {
                element.OnDestruction();
                Destroy(element.gameObject);
                ActiveCount--;
            }
        }
    }

    /// <summary>
    /// <inheritdoc cref="UOnEntityCreatedSpawner{T}"/><br></br><br></br>
    /// Additionally it uses [<seealso cref="Component.GetComponents{T}()"/>]
    /// with event calls for [<see cref="IEntityExistenceElement{T}"/>]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UOnEntityCreatedSpawnerWithListeners<T> : UOnEntityCreatedSpawner<T>
        where T : MonoBehaviour, IEntityExistenceElement<T>
    {

        private IEntityExistenceElementListener<T>[] _listeners;
        protected abstract IEntityExistenceElementListener<T>[] GetListeners();

        protected override void Awake()
        {
            base.Awake();
            _listeners = GetListeners();
        }

        protected override void OnElementCreated(in T element, in CombatEntity entity)
        {
            base.OnElementCreated(in element, in entity);

            foreach (var listener in _listeners)
            {
                listener.OnElementCreated(in element, in entity);
            }
        }

        public override void OnCombatEnd()
        {
            base.OnCombatEnd();
            Clear();
        }


        public void Clear()
        {
            foreach (var listener in _listeners)
            {
                listener.ClearEntities();
            }
        }


    }

    public interface IEntityExistenceElement<T> where T : UnityEngine.Object, IEntityExistenceElement<T>
    {
        void EntityInjection(in CombatEntity entity);
        void OnPreStartCombat();
        void OnInstantiation();
        void OnDestruction();
    }


    public interface IEntityExistenceElementListener<T> where T : UnityEngine.Object, IEntityExistenceElement<T>
    {
        void OnElementCreated(in T element, in CombatEntity entity);
        void ClearEntities();
    }
}
