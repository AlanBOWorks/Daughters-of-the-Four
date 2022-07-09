using System;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem.Player.UI
{
    public class UEffectTextHandler : MonoBehaviour
    {
        [Title("Params")]
        [SerializeField] private ReferencesHolder mainHolder;
        [SerializeField] private ReferencesHolder secondaryHolder;


        public void HandleEffect(Sprite icon)
        {
            mainHolder.SwitchIcon(icon);
            secondaryHolder.SwitchIcon(icon);
        }

        [Serializable]
        private struct ReferencesHolder
        {
            [SerializeField]
            private Image iconHolder;

            public void SwitchIcon(Sprite icon)
            {
                iconHolder.sprite = icon;
            }
        }
    }
}
