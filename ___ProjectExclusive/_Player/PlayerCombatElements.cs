using System;
using System.Collections.Generic;
using _CombatSystem;
using Characters;
using UnityEngine;

namespace _Player
{
    public class PlayerCombatElementsPools :ICombatPreparationListener, ICombatAfterPreparationListener, ICombatFinishListener
    {
        public IPlayerCombatPool<UCharacterUIHolder> CharacterUIPool { set; private get; }
        private readonly PlayerCombatDictionary _entitiesDictionary;

        public PlayerCombatElementsPools(PlayerCombatDictionary entitiesDictionary)
        {
            _entitiesDictionary = entitiesDictionary;

            CombatSystemSingleton.Invoker.SubscribeListener(this);
        }

        public void OnAfterPreparation(CombatingTeam playerEntities, CombatingTeam enemyEntities,
            CharacterArchetypesList<CombatingEntity> allEntities)
        {
            foreach (CombatingEntity entity in allEntities)
            {
                var uiHolder = CharacterUIPool.PoolDoInjection(entity);
                var element = new PlayerCombatElement(uiHolder);
                _entitiesDictionary.Add(entity,element);
            }
        }

        public void OnFinish(CombatingTeam removeEnemies)
        {
            foreach (KeyValuePair<CombatingEntity, PlayerCombatElement> pair in _entitiesDictionary)
            {
                var entity = pair.Key;
                var element = pair.Value;
                CharacterUIPool.ReturnElement(element.UIHolder);
                _entitiesDictionary.Remove(entity);
            }
        }


        public void OnBeforeStart(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
        {
            if (_entitiesDictionary.Count > 0)
            {
                foreach (KeyValuePair<CombatingEntity, PlayerCombatElement> pair in _entitiesDictionary)
                {
                    Debug.LogError($"Element in Player's Pool: {pair.Key.CharacterName}");
                }
                throw new SystemException("Elements weren't cleaned properly beforehand the Combat",
                    new IndexOutOfRangeException($"Amount of not disposed elements: {_entitiesDictionary.Count}"));
            }
               
        }
    }

    /// <summary>
    /// It's an element that shows in the combat and
    /// is exclusive to the player (generally UI elements). <br></br><br></br>
    /// The difference to a [<see cref="CombatingEntity"/>]:<br></br>
    /// [<see cref="CombatingEntity"/>] are required for
    /// the Combat.<br></br>
    /// [<see cref="PlayerCombatElement"/>] are related to the Combat (but not required for it) yet
    /// essential to them.</summary>
    public class PlayerCombatElement
    {
        public UCharacterUIHolder UIHolder;

        public PlayerCombatElement(UCharacterUIHolder uiHolder)
        {
            UIHolder = uiHolder;
        }

    }

    /// <summary>
    /// It's an element pool that exits for the player during Combat. This could be a character's UI, buttons
    /// for selecting such character as a target and similar
    /// </summary>
    public interface IPlayerCombatPool<T> where T : UnityEngine.Object
    {
        T PoolDoInjection(CombatingEntity entity);
        void ReturnElement(T element);
    }

    public class PlayerCombatDictionary : Dictionary<CombatingEntity, PlayerCombatElement> 
    {
        public PlayerCombatDictionary(int length) : base(length)
        { }
    }
}
