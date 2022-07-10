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

        [Title("Input Stances")] 
        [SerializeField] private InputActionReference supportStanceReference;
        [SerializeField] private InputActionReference attackStanceReference;
        [SerializeField] private InputActionReference defendStanceReference;

        [Title("Tempo Shortcuts")] 
        [SerializeField] private InputActionReference pauseTickingReference;

        [SerializeField] private InputActionReference endControlReference;

        private void Awake()
        {
            CombatShortcutsSingleton.InputActions = this;
        }

        public IReadOnlyList<InputActionReference> SkillShortCuts => skillShortcutReferences;
        public InputActionReference SwitchPreviousEntityShortCutElement => previousPerformerActionReference;
        public InputActionReference SwitchNextEntityShortCutElement => nextPerformerActionReference;

        public InputActionReference SupportStanceShortCutElement => supportStanceReference;
        public InputActionReference AttackStanceShortCutElement => attackStanceReference;
        public InputActionReference DefendStanceShortCutElement => defendStanceReference;

        public InputActionReference PauseTickingShortCutElement => pauseTickingReference;
        public InputActionReference EndControlShortCutElement => endControlReference;
    }

    public interface IShortcutCommandStructureRead<out T> : 
        ISkillShortcutCommandStructureRead<T>, ISwitchEntityShortcutCommandStructureRead<T>,
        ISwitchStanceShortcutCommandStructureRead<T>, ITempoShortcutCommandsStructureRead<T>
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

    public interface ISwitchStanceShortcutCommandStructureRead<out T>
    {
        T SupportStanceShortCutElement { get; }
        T AttackStanceShortCutElement { get; }
        T DefendStanceShortCutElement { get; }
    }

    public interface ITempoShortcutCommandsStructureRead<out T>
    {
        T PauseTickingShortCutElement { get; }
        T EndControlShortCutElement { get; }
    }
}
