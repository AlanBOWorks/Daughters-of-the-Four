using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatSystem.Player.UI
{
    public class UShortcutCommandsHandler : MonoBehaviour,
        IShortcutCommandStructureRead<InputActionReference>
    {
        [Title("Input References")]
        [SerializeField] private InputActionReference[] skillShortcutReferences;
        [SerializeField] private InputActionReference switchPerformerReference;


        public IReadOnlyList<InputActionReference> SkillShortCuts => skillShortcutReferences;
        public InputActionReference SwitchEntityShortCutElement => switchPerformerReference;
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
        T SwitchEntityShortCutElement { get; }
    }
}
