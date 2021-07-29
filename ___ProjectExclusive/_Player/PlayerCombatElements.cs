using System;
using System.Collections.Generic;
using _CombatSystem;
using _Team;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Player
{
    public class PlayerCombatElementsPools :ICombatPreparationListener, ICombatAfterPreparationListener, ICombatFinishListener
    {
        public IPlayerCombatPool<UCharacterUIHolder> CharacterUIPool { set; private get; }
        [ShowInInspector] 
        public readonly Dictionary<CombatingEntity, PlayerCombatElement> EntitiesDictionary;

        public PlayerCombatElementsPools()
        {
            int amountOfCharacters = UtilsCharacter.PredictedAmountOfCharactersInBattle;
            EntitiesDictionary = 
                new Dictionary<CombatingEntity, PlayerCombatElement>(amountOfCharacters);
        }

        public void OnAfterPreparation(CombatingTeam playerEntities, CombatingTeam enemyEntities,
            CharacterArchetypesList<CombatingEntity> allEntities)
        {
            DoPool(playerEntities, true);
            DoPool(enemyEntities,false);

            void DoPool(CombatingTeam team, bool isPlayer)
            {
                foreach (CombatingEntity entity in team)
                {
                    var uiHolder = CharacterUIPool.PoolDoInjection(entity, isPlayer);
                    var element = new PlayerCombatElement(uiHolder);
                    EntitiesDictionary.Add(entity, element);
                }
            }
        }

        public void OnCombatFinish(CombatingEntity lastEntity, bool isPlayerWin)
        {
            foreach (KeyValuePair<CombatingEntity, PlayerCombatElement> pair in EntitiesDictionary)
            {
                var element = pair.Value;
                CharacterUIPool.ReturnElement(element.UIHolder);
            }

            EntitiesDictionary.Clear();
        }


        public void OnBeforeStart(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
        {
            if (EntitiesDictionary.Count > 0)
            {
                foreach (KeyValuePair<CombatingEntity, PlayerCombatElement> pair in EntitiesDictionary)
                {
                    Debug.LogError($"Element in Player's Pool: {pair.Key.CharacterName}");
                }
                throw new SystemException("Elements weren't cleaned properly beforehand the Combat",
                    new IndexOutOfRangeException($"Amount of not disposed elements: {EntitiesDictionary.Count}"));
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

        public UTargetButton GetTargetButton()
        {
            return UIHolder.TargetButton;
        }
    }

    /// <summary>
    /// It's an element pool that exits for the player during Combat. This could be a character's UI, buttons
    /// for selecting such character as a target and similar
    /// </summary>
    public interface IPlayerCombatPool<T> where T : UnityEngine.Object
    {
        T PoolDoInjection(CombatingEntity entity, bool isPlayer);
        void ReturnElement(T element);
    }

}
