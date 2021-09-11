using System;
using System.Collections.Generic;
using _CombatSystem;
using _Team;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Player
{
    public class PlayerCombatUIElements : NestedTeamData<PlayerCombatUIElement>, ICombatAfterPreparationListener,
        IPersistentInjectorHolders
    {
        [ShowInInspector]
        public readonly Dictionary<CombatingEntity, PlayerCombatUIElement> Dictionary;

        public PlayerCombatUIElements() : base()
        {
            Dictionary = new Dictionary<CombatingEntity, PlayerCombatUIElement>();
        }

        public void OnAfterPreparation(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
        {
            UtilsCharacterArchetypes.InjectInDictionary(Dictionary,playerEntities,PlayerData);
            UtilsCharacterArchetypes.InjectInDictionary(Dictionary,enemyEntities,EnemyData);
        }

        public override PlayerCombatUIElement GenerateElement()
            => new PlayerCombatUIElement();

        public ICharacterArchetypesData<IPersistentElementInjector> GetPlayerInjectors()
            => PlayerData;
        public ICharacterArchetypesData<IPersistentElementInjector> GetEnemyInjectors()
            => EnemyData;
    }

    /// <summary>
    /// It's an element that shows in the combat and
    /// is exclusive to the player (generally UI elements). <br></br><br></br>
    /// The difference to a [<see cref="CombatingEntity"/>]:<br></br>
    /// [<see cref="CombatingEntity"/>] are required for
    /// the Combat.<br></br>
    /// [<see cref="PlayerCombatUIElement"/>] are related to the Combat (but not required for it) yet
    /// essential to them.</summary>
    public sealed class PlayerCombatUIElement : IPersistentElementInjector, IPersistentEntitySwitchListener
    {
        public UCharacterUIHolder UIHolder;

        public UTargetButton GetTargetButton()
        {
            return UIHolder.TargetButton;
        }

        public void DoInjection(EntityPersistentElements persistentElements)
        {
            UIHolder.DoInjection(persistentElements);
        }

        public void OnEntitySwitch(CombatingEntity entity)
        {
            UIHolder.OnEntitySwitch(entity);
        }
    }
}
