using System.Collections.Generic;
using _Team;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CombatSystem
{
    public abstract class DictionaryEntityElements<T> : Dictionary<CombatingEntity, T>,
        ITeamsData<ICharacterArchetypesData<T>>,
        ICombatPreparationListener
    {
        protected DictionaryEntityElements()
        {
            PlayerData 
                = new ElementsHolder(GenerateElement(), GenerateElement(), GenerateElement());
            EnemyData
                = new ElementsHolder(GenerateElement(), GenerateElement(), GenerateElement());
        }

        [ShowInInspector]
        public ICharacterArchetypesData<T> PlayerData { get; }
        [ShowInInspector]
        public ICharacterArchetypesData<T> EnemyData { get; }
        public void OnBeforeStart(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
        {
            Clear();
            UtilsCharacterArchetypes.InjectInDictionary(this, playerEntities, PlayerData);
            UtilsCharacterArchetypes.InjectInDictionary(this, enemyEntities, EnemyData);


            if (PlayerData is ICharacterArchetypesData<IEntitySwitchListener> playerSwitchers)
                UtilsCharacter.DoSwitch(playerSwitchers, playerEntities);
            if (EnemyData is ICharacterArchetypesData<IEntitySwitchListener> enemySwitchers)
                UtilsCharacter.DoSwitch(enemySwitchers, enemyEntities);
        }

        public T GetElement(EnumCharacter.RoleArchetype role, bool isPlayer)
        {
            var dataHolder = (isPlayer) ? PlayerData : EnemyData;
            return UtilsCharacter.GetElement(dataHolder, role);
        }

        protected abstract T GenerateElement();

        private class ElementsHolder : CharacterArchetypes<T>
        {
            public ElementsHolder(T vanguard,T attacker,T support)
            {
                Vanguard = vanguard;
                Attacker = attacker;
                Support = support;
            }
        }
    }
}
