using CombatSystem._Core;
using CombatSystem.Entity;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace CombatSystem.Team
{
    /// <summary>
    /// [ Used for spawning all members as [<see cref="T"/>] elements ] <br></br><br></br>
    /// Spawn elements of [<see cref="T"/>] by Instantiation through prefab references;<br></br>
    /// It also saves the dictionaries keeps dictionaries for all entities and each team of
    /// type(<see cref="CombatEntity"/>,<see cref="T"/>).<br></br><br></br>
    /// Note: <br></br>
    /// - This doesn't lazy subscribe to [<seealso cref="CombatEntityEventsHolder"/>]. <br></br>
    /// - It also does a [<seealso cref="Component.GetComponents{ITeamElementSpawnListener}()"/>]
    /// where T : [<seealso cref="ITeamElementSpawnListener{T}"/>] >>>><br></br>
    /// >>>> By adding [<seealso cref="ITeamElementSpawnListener{T}"/>] listeners will be called during elements spawning.
    /// </summary>
    public abstract class UTeamElementSpawner<T> : MonoBehaviour, ICombatStatesListener
        where T: Object

    {
        [TitleGroup("Params")] 
        [SerializeField, HideInPlayMode]
        private bool hidePrefabs = true;


        [Tooltip("False: FlexType is also Main Role")]
        [SerializeField, HideInPlayMode] 
        private bool isFlexMainRole = true;

        [Title("Params")]
        [SerializeField,HideInPlayMode]
        private PrefabReferencesHolder prefabReferences = new PrefabReferencesHolder();


        protected TeamPrefabReferences _playerTeamReferences;
        protected TeamPrefabReferences _enemyTeamReferences;

        [ShowInInspector, HideInEditorMode] 
        private IReadOnlyDictionary<CombatEntity, T> _dictionary;
        public IReadOnlyDictionary<CombatEntity, T> GetDictionary() => _dictionary;

        [ShowInInspector,HideInEditorMode]
        private ITeamElementSpawnListener<T>[] _listeners;

        private void Awake()
        {
            var prefabReferencesHolder = prefabReferences;
            prefabReferencesHolder.ExtractPlayer(out var playerMainReferences,out var playerOffReferences);
            _playerTeamReferences = new TeamPrefabReferences(this,playerMainReferences,playerOffReferences);
            prefabReferencesHolder.ExtractEnemy(out var enemyMainReferences,out var enemyOffReferences);
            _enemyTeamReferences = new TeamPrefabReferences(this,enemyMainReferences,enemyOffReferences);

            _dictionary = ConcatDualDictionary<CombatEntity, T>.GetFirstOrNewConcatDictionary(
                _playerTeamReferences.GetDictionary(), _enemyTeamReferences.GetDictionary());


            _listeners = GetComponents<ITeamElementSpawnListener<T>>();

            if(hidePrefabs)
                prefabReferences.HideAll();
        }

        private void CallPreStartEvents()
        {
            foreach (var listener in _listeners)
            {
                listener.OnAfterElementsCreated(this);
            }
        }
        private void CallIterationEvents(CombatEntity entity, T element, int index)
        {
            foreach (var listener in _listeners)
            {
                listener.OnElementCreated(element, entity, index);
            }
        }

        private void CallEndCombatEvents()
        {
            foreach (var listener in _listeners)
            {
                listener.OnCombatEnd();
            }
        }


        public void OnCombatEnd()
        {
            _playerTeamReferences.DeSpawn();
            _enemyTeamReferences.DeSpawn();
            CallEndCombatEvents();
        }

        public virtual void OnCombatFinish(bool isPlayerWin)
        {
        }

        public virtual void OnCombatQuit()
        {
        }

        public virtual void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            HandlePlayer();
            HandleEnemy();
            CallPreStartEvents();


            void HandlePlayer()
            {
                _playerTeamReferences.HandleTeam(playerTeam, isFlexMainRole);
            }

            void HandleEnemy()
            {
                _enemyTeamReferences.HandleTeam(enemyTeam, isFlexMainRole);
            }
        }


        public virtual void OnCombatStart()
        {
        }

        private void DoCreateElement(CombatEntity entity, T element, int index, bool isPlayerElement)
        {
            CallIterationEvents(entity, element, index);
            OnCreateElement(entity,element, index);
            OnCreateElement(entity, element, isPlayerElement);
        }
        

        protected virtual void OnCreateElement(CombatEntity entity, T element,
            int index)
        {

        }
        protected virtual void OnCreateElement(CombatEntity entity, T element, bool isPlayerElement)
        {

        }


        protected class TeamPrefabReferences
        {
            public TeamPrefabReferences(UTeamElementSpawner<T> spawner,PrefabInstantiationHandler<T> main) 
                :this(spawner,main, main)
            {
            }

            public TeamPrefabReferences(UTeamElementSpawner<T> spawner,PrefabInstantiationHandler<T> main, PrefabInstantiationHandler<T> off)
            {
                _spawner = spawner;
                MainMembersPrefab = main;
                OffMembersPrefab = off;

                _mainMembersDictionary = new PrefabDictionary<CombatEntity, T>();
                _offMembersDictionary = new PrefabDictionary<CombatEntity, T>();
                _dualDictionary = new ConcatDualDictionary<CombatEntity, T>(_mainMembersDictionary,_offMembersDictionary);

            }

            private readonly UTeamElementSpawner<T> _spawner;
            public readonly PrefabInstantiationHandler<T> MainMembersPrefab;
            public readonly PrefabInstantiationHandler<T> OffMembersPrefab;

            [ShowInInspector,HideInEditorMode]
            private readonly PrefabDictionary<CombatEntity, T> _mainMembersDictionary;
            private readonly PrefabDictionary<CombatEntity, T> _offMembersDictionary;
            private readonly ConcatDualDictionary<CombatEntity, T> _dualDictionary;

            public IReadOnlyDictionary<CombatEntity, T> GetDictionary() => _dualDictionary;
            public IReadOnlyDictionary<CombatEntity, T> GetMainDictionary() => _mainMembersDictionary;
            public IReadOnlyDictionary<CombatEntity, T> GetOffDictionary() => _offMembersDictionary;
           

            public void DeSpawn()
            {
                _mainMembersDictionary.ClearDestroy();
                _offMembersDictionary.ClearDestroy();
            }

            public void HandleTeam(CombatTeam team, bool isFlexMainRole)
            {
                IEnumerable<CombatEntity> mainRoles;
                IEnumerable<CombatEntity> offRoles;
                if (isFlexMainRole)
                    FlexIsMain(team, out mainRoles, out offRoles);
                else
                    FlexIsOff(team, out mainRoles, out offRoles);

                bool isPlayerTeam = team.IsPlayerTeam;

                HandleIterator(mainRoles, MainMembersPrefab, _mainMembersDictionary, isPlayerTeam);
                HandleIterator(offRoles, OffMembersPrefab, _offMembersDictionary, isPlayerTeam);
            }
            private static void FlexIsMain(CombatTeam team,
                out IEnumerable<CombatEntity> mainRoles,
                out IEnumerable<CombatEntity> offRole)
            {
                mainRoles = team.GetMainRoles();
                offRole = team.GetOffRoles();
            }
            private static void FlexIsOff(CombatTeam team, 
                out IEnumerable<CombatEntity> mainRoles,
                out IEnumerable<CombatEntity> offRole)
            {
                var teamEntities = team.GetAllEntities();
                mainRoles = UtilsTeam.GetEnumerable((ITeamTrinityStructureRead<CombatEntity>) teamEntities);
                offRole = UtilsTeam.GetOffElementWithFlex(teamEntities);
            }

            private void HandleIterator(IEnumerable<CombatEntity> members, 
                PrefabInstantiationHandler<T> handler, Dictionary<CombatEntity,T> dictionary, bool isPlayerTeam)
            {
                int i = 0;
                foreach (var member in members)
                {
                    if(member == null)
                    {
                        continue;
                    }

                    var element = handler.SpawnElement();
                    dictionary.Add(member,element);
                    _spawner.DoCreateElement(member,element, i, isPlayerTeam);
                    i++;
                }
            }
        }



        [Serializable]
        private class PrefabReferencesHolder
        {
            [SerializeField, TitleGroup("Params")] private bool spawnMainsMembers = true;
            [SerializeField, TitleGroup("Params")] private bool spawnOffMembers = true;

            [SerializeField, TitleGroup("Main"),ShowIf("spawnMainsMembers")]
            public PrefabInstantiationHandler<T> playerMainPrefab = new PrefabInstantiationHandler<T>();
            [SerializeField, TitleGroup("Main"), ShowIf("spawnMainsMembers")]
            public PrefabInstantiationHandler<T> enemyMainPrefab = new PrefabInstantiationHandler<T>();


            [SerializeField, TitleGroup("Off"), ShowIf("spawnOffMembers")]
            public PrefabInstantiationHandler<T> playerOffPrefab = new PrefabInstantiationHandler<T>();
            [SerializeField, TitleGroup("Off"), ShowIf("spawnOffMembers")]
            public PrefabInstantiationHandler<T> enemyOffPrefab = new PrefabInstantiationHandler<T>();

            public void ExtractPlayer(
                out PrefabInstantiationHandler<T> playerMainReferences,
                out PrefabInstantiationHandler<T> playerOffReferences)
            {
                playerMainReferences = playerMainPrefab;
                playerOffReferences = playerOffPrefab.IsValid() ? playerOffPrefab : playerMainReferences;
            }
            public void ExtractEnemy(
                out PrefabInstantiationHandler<T> enemyMainReferences,
                out PrefabInstantiationHandler<T> enemyOffReferences)
            {
                enemyMainReferences = enemyMainPrefab.IsValid() ? enemyMainPrefab : playerMainPrefab;
                enemyOffReferences = enemyOffPrefab.IsValid() ? enemyOffPrefab : enemyMainReferences;
            }

            public void HideAll()
            {
                if (spawnMainsMembers)
                {
                    Disable(playerMainPrefab);
                    Disable(enemyMainPrefab);
                }
                if (spawnOffMembers)
                {

                    Disable(playerOffPrefab);
                    Disable(enemyOffPrefab);
                }
            }

            private static void Disable(PrefabInstantiationHandler<T> handler)
            {
                var prefab = handler.GetPrefab();
                if (prefab is MonoBehaviour mono) mono.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Interfaces for being called during [<see cref="UTeamElementSpawner{T}.OnCombatPreStarts"/>]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITeamElementSpawnListener<T> where T : UnityEngine.Object
    {
        void OnElementCreated(T element, CombatEntity entity, int index);
        void OnAfterElementsCreated(UTeamElementSpawner<T> holder);
        void OnCombatEnd();

    }
}
