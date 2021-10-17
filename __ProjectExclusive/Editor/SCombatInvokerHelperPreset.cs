using __ProjectExclusive.Enemy;
using __ProjectExclusive.Player;
using CombatSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Editor
{
    [CreateAssetMenu(fileName = "CombatInvoker - N [Preset]",
        menuName = "Editor/Combat/Invoker Preset")]
    public class SCombatInvokerHelperPreset : ScriptableObject
    {
        [SerializeField, HorizontalGroup()] 
        public PlayerCharactersSelector playerCharacters = new PlayerCharactersSelector();
        [SerializeField, HorizontalGroup()] 
        public EnemyCombatProvider enemyCombatProvider = new EnemyCombatProvider();
    }
}
