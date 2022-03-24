using Sirenix.OdinInspector;
using UnityEngine;

namespace DataHolders
{
    public sealed class CountableValue
    {
        [ShowInInspector]
        public float Value { get; private set; }
        [ShowInInspector]
        public int InteractedAmount { get; private set; }

        public void Interact(in float variation)
        {
            Value += variation;
            InteractedAmount++;
        }

        public void Reset()
        {
            Value = 0;
            InteractedAmount = 0;
        }
    }
}
