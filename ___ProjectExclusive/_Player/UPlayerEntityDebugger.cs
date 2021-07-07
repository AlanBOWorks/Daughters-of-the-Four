using Sirenix.OdinInspector;
using UnityEngine;

namespace _Player
{
    public class UPlayerEntityDebugger : MonoBehaviour
    {
#if UNITY_EDITOR
        [ShowInInspector, DisableInEditorMode]
        private PlayerEntitySingleton _playerEntity = PlayerEntitySingleton.Instance;

#endif
    }
}
