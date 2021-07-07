using System;
using System.Collections.Generic;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Player
{
    public class UPlayerCharactersSelector : MonoBehaviour, 
        IPlayerArchetypes<SPlayerCharacterEntityVariable>
    {
        [Title("Selections")]
        [SerializeField] private SPlayerCharacterEntityVariable _frontLiner;
        [SerializeField] private SPlayerCharacterEntityVariable _midLiner;
        [SerializeField] private SPlayerCharacterEntityVariable _backLiner;

        public static void DoSelect(IPlayerArchetypesData<SPlayerCharacterEntityVariable> selected)
        {
            PlayerEntitySingleton.SelectedCharacters = selected;
        }
        [Button,DisableInEditorMode]
        public void DoSelectOfCurrent()
        {
            if(!CharacterArchetypes.IsValid(this))
                throw new ArgumentException("Invalid selected characters; Some roles are not selected");

            DoSelect(this);
        }

        private void Start()
        {
            if (CharacterArchetypes.IsValid(this))
            {
                DoSelectOfCurrent();
            }
        }

        public SPlayerCharacterEntityVariable FrontLiner
        {
            get => _frontLiner;
            set => _frontLiner = value;
        }

        public SPlayerCharacterEntityVariable MidLiner
        {
            get => _midLiner;
            set => _midLiner = value;
        }

        public SPlayerCharacterEntityVariable BackLiner
        {
            get => _backLiner;
            set => _backLiner = value;
        }
    }


}
