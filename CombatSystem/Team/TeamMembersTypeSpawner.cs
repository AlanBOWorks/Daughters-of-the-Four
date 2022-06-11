using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace CombatSystem.Team
{
    /// <summary>
    /// Handles instantiations elements of [T] for [<see cref="CombatTeam"/>]0s members
    /// </summary>
    public abstract class TeamMembersTypeSpawner<T> where T : Object
    {
        [SerializeField]
        protected PrefabHolder prefabHolder = new PrefabHolder();
        [SerializeField] 
        private bool doPooling = true;


        private IEnumerable<CombatEntity> _entities;
        [ShowInInspector, DisableInEditorMode]
        private Queue<T> _activeElements;
        [ShowInInspector,DisableInEditorMode]
        private Stack<T> _pool;

        public bool IsValid() => prefabHolder.IsValid();


        public T GetPrefab() => prefabHolder.GetPrefab();
        private void Awake()
        {
            if(_activeElements != null) return;

            _activeElements = new Queue<T>();
            if(doPooling)
                _pool = new Stack<T>();
        }


        public void OnCombatPrepares(IEnumerable<CombatEntity> members, Action<KeyValuePair<CombatEntity,T>> onCreationCallback)
        {
            Awake();
            _entities = members;
            DoInstantiation(onCreationCallback);
        }

        public void OnCombatFinish(Action<T> onDisableCallback)
        {
            while (_activeElements.Count > 0)
            {
                var element = _activeElements.Dequeue();
                if (doPooling)
                {
                    _pool.Push(element);
                    onDisableCallback?.Invoke(element);
                }
                else
                    Object.Destroy(element);
            }
        }

        private void DoInstantiation(Action<KeyValuePair<CombatEntity, T>> onCreationCallback)
        {
            foreach (var entity in _entities)
            {
                var element = PoolOrInstantiate();
                _activeElements.Enqueue(element);

                OnInstantiationElement(element,entity, _activeElements.Count);
                onCreationCallback?.Invoke(new KeyValuePair<CombatEntity, T>(entity, element));
            }


            T PoolOrInstantiate()
            {
                return _pool != null && _pool.Count > 0
                    ? _pool.Pop()
                    : prefabHolder.SpawnElement();
            }
        }


        protected abstract void OnInstantiationElement(T element, CombatEntity entity, int count);



        [Serializable]
        protected sealed class PrefabHolder : PrefabInstantiationHandler<T>
        {
            
        }
    }

    public abstract class TeamAlimentHolder<THandler, TElement> :
        ITeamAlimentStructureRead<TeamMembersTypeSpawner<TElement>>
        where THandler : ICombatEntityElementHolder<TElement>
        where TElement : MonoBehaviour
    {
        [SerializeField]
        private RolesSpawner mainRoles = new RolesSpawner();
        [SerializeField]
        private RolesSpawner secondaryRoles = new RolesSpawner();
        [SerializeField]
        private RolesSpawner thirdRoles = new RolesSpawner();


        public TeamMembersTypeSpawner<TElement> MainRole => mainRoles;
        public TeamMembersTypeSpawner<TElement> SecondaryRole => secondaryRoles;
        public TeamMembersTypeSpawner<TElement> ThirdRole => thirdRoles;

        public void DoInjection(THandler handler)
        {
            foreach (var spawner in GetEnumerable())
            {
                if (!spawner.IsValid()) return;

                spawner.Injection(handler);
                var prefabReference = spawner.GetPrefab();
                prefabReference.gameObject.SetActive(false);
            }
        }

        public void DoInjection(CombatTeam team)
        {
            mainRoles.OnCombatPrepares(team.GetMainRoles(), null);
            secondaryRoles.OnCombatPrepares(team.GetSecondaryRoles(), null);
            thirdRoles.OnCombatPrepares(team.GetThirdRoles(), null);
        }

        public void OnFinish(Action<TElement> hideTracker)
        {
            foreach (var spawner in GetEnumerable())
            {
                spawner.OnCombatFinish(hideTracker);
            }
        }

        private IEnumerable<RolesSpawner> GetEnumerable()
        {
            yield return mainRoles;
            yield return secondaryRoles;
            yield return thirdRoles;
        }

        [Serializable]
        private class RolesSpawner : TeamMembersTypeSpawner<TElement>
        {
            private THandler _handler;
            public void Injection(THandler handler) => _handler = handler;

            protected override void OnInstantiationElement(TElement element, CombatEntity entity, int count)
            {
                _handler.HandleElementInjection(entity, element);
            }
        }

    }


    public abstract class UTeamDualAlimentHandler<TElement> : MonoBehaviour,
        IOppositionTeamStructureRead<ITeamAlimentStructureRead<PrefabInstantiationHandlerPool<TElement>>>
        where TElement : MonoBehaviour
    {
        private void Awake()
        {
            Instantiate(PlayerTeamType);
            Instantiate(EnemyTeamType);
        }

        public abstract ITeamAlimentStructureRead<PrefabInstantiationHandlerPool<TElement>> PlayerTeamType { get; }
        public abstract ITeamAlimentStructureRead<PrefabInstantiationHandlerPool<TElement>> EnemyTeamType { get; }

        private static void Instantiate(ITeamAlimentStructureRead<PrefabInstantiationHandlerPool<TElement>> holder)
        {
            var enumerable = UtilsTeam.GetEnumerable(holder);
            foreach (var handler in enumerable)
            {
                handler?.Instantiations();
            }
        }

        public void HandleElements(CombatTeam playerTeam, CombatTeam enemyTeam, Action<CombatEntity, TElement> onCreationFallback)
        {
            HandleElements(PlayerTeamType,playerTeam, onCreationFallback);
            HandleElements(EnemyTeamType,enemyTeam, onCreationFallback);
        }

        public void HandlePlayerElements(CombatTeam playerTeam, Action<CombatEntity, TElement> onCreationFallback) 
            => HandleElements(PlayerTeamType,playerTeam,onCreationFallback);
        public void HandleEnemyElements(CombatTeam enemyTeam, Action<CombatEntity, TElement> onCreationFallback)
            => HandleElements(EnemyTeamType, enemyTeam, onCreationFallback);


        private static void HandleElements(ITeamAlimentStructureRead<
            PrefabInstantiationHandlerPool<TElement>> holder, 
            CombatTeam team,
            Action<CombatEntity,TElement> onCreationFallback)
        {
            var enumerable = UtilsTeam.GetEnumerable( team.GetRolesAliments(), holder);
            foreach (var (members, spawner) in enumerable)
            {
                if(spawner == null || members == null) continue;

                spawner.DoPopKeys(members, onCreationFallback);
            }
        }

        public void ReturnElements(Action<TElement> onReturnElementFallBack)
        {
            ReturnElements(PlayerTeamType,onReturnElementFallBack);
            ReturnElements(EnemyTeamType,onReturnElementFallBack);
        }
        private static void ReturnElements(ITeamAlimentStructureRead<PrefabInstantiationHandlerPool<TElement>> holder,
            Action<TElement> onReturnElementFallBack)
        {
            var enumerable = UtilsTeam.GetEnumerable(holder);
            foreach (var handlerPool in enumerable)
            {
                handlerPool.ReturnActives(onReturnElementFallBack);
            }
        }
    }


    public interface ICombatEntityElementHolder<in T>
    {
        void HandleElementInjection(CombatEntity entity, T element);
    }
}
