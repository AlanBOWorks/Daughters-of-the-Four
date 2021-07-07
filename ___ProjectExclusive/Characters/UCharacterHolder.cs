using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Characters
{
    public class UCharacterHolder : MonoBehaviour
    {
        [Title("References")]
        public Transform meshTransform = null;

        public void Injection(CombatingEntity entity)
        {
            Entity = entity;
            BaseStats = entity.CombatStats;
            entity.Holder = this;
        }
        [ShowInInspector,DisableInEditorMode]
        public CombatingEntity Entity { get; private set; }
        public ICharacterFullStats BaseStats { get; private set; }


        public Transform GetTooltipTransform()
        {
            return (meshTransform) ? meshTransform : transform;
        }
    }
}
