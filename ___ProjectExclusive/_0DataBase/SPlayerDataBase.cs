using System;
using System.Collections.Generic;
using Characters;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Stats;
using UnityEditor;
using UnityEngine;

namespace ___ProjectExclusive
{
    public class PlayerDataBaseWindow : OdinEditorWindow
    {
        [MenuItem("Game Menus/Data Bases/Player")]
        private static void OpenWindow()
        {
            GetWindow<PlayerDataBaseWindow>().Show();
        }

        public PlayerDataBase PlayerDataBase = DataBaseSingleton.Instance.PlayerDataBase;
    }
    [CreateAssetMenu(fileName = "Player DataBase [Singleton]",
        menuName = "Singleton/Player DataBase")]
    public class SPlayerDataBase : ScriptableObject
    {
        [SerializeField] private PlayerDataBase dataBase = DataBaseSingleton.Instance.PlayerDataBase;

        [Button]
        private void InjectSelf()
        {
            DataBaseSingleton.Instance.PlayerDataBase.serializationReference = this;
        }

        private void Awake()
        {
            InjectSelf();
        }
    }

    [Serializable]
    public class PlayerDataBase 
    {
        [SerializeReference] 
        public SPlayerDataBase serializationReference = null;

        [SerializeField, HideInEditorMode, HideInPlayMode]
        private List<SPlayerCharacterEntityVariable> charactersKey 
            = new List<SPlayerCharacterEntityVariable>();
        [ShowInInspector]
        private Dictionary<SPlayerCharacterEntityVariable, CharacterData> _charactersData;
        [Button]
        public void InjectVariable(SPlayerCharacterEntityVariable variable)
        {
            if (charactersKey.Contains(variable)) return;
            charactersKey.Add(variable);
            _charactersData.Add(variable, new CharacterData(variable));
        }

        public void RemoveVariable(SPlayerCharacterEntityVariable variable)
        {
            _charactersData.Remove(variable);
            charactersKey.Remove(variable);
        }

        [Button]
        private void InjectVariables(SPlayerCharacterEntityVariable[] variables)
        {
            foreach (var entityVariable in variables)
            {
                InjectVariable(entityVariable);
            }
        }


        public PlayerDataBase()
        {
            _charactersData = new Dictionary<SPlayerCharacterEntityVariable, CharacterData>(4);
        }

        internal class CharacterData
        {
            public SPlayerCharacterEntityVariable variable;
            [HorizontalGroup("Stats")]
            public PlayerCombatStats initialStats;
            [HorizontalGroup("Stats")]
            [GUIColor(.5f,.9f, .8f)]
            public PlayerCombatStats growCombatStats;
            public CharacterUpgradeStats upgradedStats;

            public CharacterData(SPlayerCharacterEntityVariable variable)
            {
                this.variable = variable;
                initialStats = variable.InitialStats;
                growCombatStats = variable.GrowStats;
                upgradedStats = variable.UpgradedStats;
            }
        }
    }
}
