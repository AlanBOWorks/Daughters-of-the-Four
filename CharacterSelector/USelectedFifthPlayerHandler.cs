using System;
using CombatSystem.Entity;
using Lore.Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CharacterSelector
{
    public class USelectedFifthPlayerHandler : MonoBehaviour
    {
        [SerializeField] private USelectedCharactersHolder holder;

        [SerializeField, InlineEditor()] private SCharacterLoreHolder fifthLore;
        [SerializeField, InlineEditor()] private SPlayerPreparationEntity fifthCharacterPreset;


        private void Start()
        {
            holder.SelectCharacter(fifthLore,fifthCharacterPreset);
        }
    }
}
