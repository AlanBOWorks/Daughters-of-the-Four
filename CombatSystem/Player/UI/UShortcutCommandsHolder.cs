using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatSystem.Player.UI
{
    public class UShortcutCommandsHolder : MonoBehaviour,
        IShortcutCommandStructureRead<InputActionReference>
    {
        [Title("Input Entities")]
        [SerializeField] private InputActionReference previousPerformerActionReference;
        [SerializeField] private InputActionReference nextPerformerActionReference;

        [Title("Input Skills")]
        [SerializeField] private InputActionReference[] skillShortcutReferences;
        


        public IReadOnlyList<InputActionReference> SkillShortCuts => skillShortcutReferences;
        public InputActionReference SwitchPreviousEntityShortCutElement => previousPerformerActionReference;
        public InputActionReference SwitchNextEntityShortCutElement => nextPerformerActionReference;
    }

    public interface IShortcutCommandStructureRead<out T> : 
        ISkillShortcutCommandStructureRead<T>, ISwitchEntityShortcutCommandStructureRead<T>
    { }

    public interface ISkillShortcutCommandStructureRead<out T>
    {
        IReadOnlyList<T> SkillShortCuts { get; }
    }

    public interface ISwitchEntityShortcutCommandStructureRead<out T>
    {
        T SwitchPreviousEntityShortCutElement { get; }
        T SwitchNextEntityShortCutElement { get; }
    }
}
