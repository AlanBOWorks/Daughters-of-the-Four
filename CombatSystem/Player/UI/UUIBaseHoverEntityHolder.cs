using CombatSystem.Entity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UUIBaseHoverEntityHolder : UUIHoverEntityBase, IEntityExistenceElement<UUIBaseHoverEntityHolder>
    {
        [Title("References")]
        [SerializeField] private UVitalityInfo healthInfo;
        public UVitalityInfo GetHealthInfo() => healthInfo;

        protected override Transform GetFollowTransform(ICombatEntityBody body)
        {
            return body.BaseRootType;
        }
    }
}
