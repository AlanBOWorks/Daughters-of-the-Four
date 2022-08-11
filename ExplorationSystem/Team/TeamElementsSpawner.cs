
using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ExplorationSystem.Team
{
    /// <summary>
    /// Spawn elements of the team as [<seealso cref="ITeamFlexStructureRead{T}"/>] kind of structure
    /// </summary>
    /// <typeparam name="TKey">Type: [<seealso cref="ICombatEntityProvider"/>]</typeparam>
    /// <typeparam name="TElement">Type: [<seealso cref="Component"/>]</typeparam>
    [Serializable]
    public class TeamPrefabElementsSpawner <TKey, TElement> : 
        ITeamFlexStructureRead<TElement>, ITeamFlexPositionStructureRead<TElement>
        where TKey : ICombatEntityProvider where TElement : Component
    {
        [SerializeField] 
        private Transform instantiateOnParent;

        [SerializeField] 
        private TElement prefabHolder;
        [SerializeField, HideInPlayMode] 
        private bool usePrefabForVanguard = true;

        private TElement _vanguardType;
        private TElement _attackerType;
        private TElement _supportType;
        private TElement _flexType;
        
        public TElement VanguardType => _vanguardType;
        public TElement AttackerType => _attackerType;
        public TElement SupportType => _supportType;
        public TElement FlexType => _flexType;
        
        public TElement FrontLineType => _vanguardType;
        public TElement MidLineType => _attackerType;
        public TElement BackLineType => _supportType;
        public TElement FlexLineType => _flexType;

        public TElement GetPrefabElement() => prefabHolder;


        public void DoInstantiations()
        {
            _vanguardType = usePrefabForVanguard 
                ? GetPreparedPrefabElement() 
                : SpawnObject();
            _attackerType = SpawnObject();
            _supportType = SpawnObject();
            _flexType = SpawnObject();


            TElement GetPreparedPrefabElement()
            {
                prefabHolder.transform.SetParent(instantiateOnParent);
                return prefabHolder;
            }
            TElement SpawnObject() => Object.Instantiate(prefabHolder, instantiateOnParent);
        }
        

        public IEnumerable<TElement> GetEnumerable()
        {
            yield return _vanguardType;
            yield return _attackerType;
            yield return _supportType;
            yield return _flexType;
        }

        public TElement GetElement(TKey key)
        {
            var role = key.GetAreaData().RoleType;
            return UtilsTeam.GetElement(role, this);
        }
    }
}
