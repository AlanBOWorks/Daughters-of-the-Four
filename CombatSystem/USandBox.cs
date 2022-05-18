using CombatSystem.Entity;
using CombatSystem.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class USandBox : MonoBehaviour
    {
        [SerializeReference] private SPlayerPreparationEntity[] serializedReference;

        [Button]
        private void TrySerialize(SPlayerPresetTeam preset)
        {
            serializedReference = preset.GetPresetCharacters();
        }
    }

}
