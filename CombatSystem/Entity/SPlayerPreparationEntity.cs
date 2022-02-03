using UnityEngine;

namespace CombatSystem.Entity
{
    [CreateAssetMenu(menuName = "Combat/Entities/[Player] Preparation",
        fileName = "N " + PlayerAssetPrefixName)]
    public sealed class SPlayerPreparationEntity : SPreparationEntity
    {
        private const string PlayerAssetPrefixName = "[PLAYER Preparation Entity]";
        protected override string AssetPrefix()
        {
            return PlayerAssetPrefixName;
        }
    }
}
