using System;
using Characters;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace _CombatSystem
{
    public class UCombatSystemDebugger : MonoBehaviour
    {
#if UNITY_EDITOR
        [ShowInInspector,DisableInEditorMode,TabGroup("Combat")]
        private CombatSystemSingleton _system = CombatSystemSingleton.Instance;
        [ShowInInspector, DisableInEditorMode, TabGroup("Prefabs")]
        private CharacterSystemSingleton _characters = CharacterSystemSingleton.Instance;

#else
        private void Awake()
        {
            Destroy(this);
        }
#endif
    }
}
