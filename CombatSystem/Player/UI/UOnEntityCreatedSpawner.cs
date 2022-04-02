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
        public IReadOnlyDictionary<CombatEntity, T> ElementsDictionary => ActiveElementsDictionary;

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
                foreach (var member in team)
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

        


        [Serializable]
        protected sealed class EntityElementSpawner
        {
            [SerializeField] private Transform instantiationParent;
            [SerializeField] private T elementEntityPrefab;
            public int ActiveCount { get; private set; }


            public void Disable()
            {
                elementEntityPrefab.gameObject.SetActive(false);
            }

            public T GenerateElement(in CombatEntity entity)
            {
                T element = Instantiate(elementEntityPrefab, instantiationParent);
                element.EntityInjection(entity);
                element.ShowElement();
                ActiveCount++;


#if UNITY_EDITOR
                element.name = entity.GetProviderEntityName() + " - UI Hover [Group] (Clone)";
#endif

                return element;
            }

            public void PushElement(in T element)
            {
                Destroy(element);
                ActiveCount--;
            }
        }

        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            foreach (var element in ActiveElementsDictionary)
            {
                element.Value.OnPreStartCombat();
            }
        }

        public void OnCombatStart()
        {
        }

        public void OnCombatFinish()
        {
        }

        public void OnCombatQuit()
        {
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
    }

    public interface IEntityExistenceElement<T> where T : UnityEngine.Object, IEntityExistenceElement<T>
    {
        void EntityInjection(in CombatEntity entity);
        void OnPreStartCombat();
        void ShowElement();
        void HideElement();
    }


    public interface IEntityExistenceElementListener<T> where T : UnityEngine.Object, IEntityExistenceElement<T>
    {
        void OnElementCreated(in T element, in CombatEntity entity);
        void ClearEntities();
    }
}
