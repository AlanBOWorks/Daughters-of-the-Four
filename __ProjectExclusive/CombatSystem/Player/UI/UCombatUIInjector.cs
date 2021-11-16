using System;
using UnityEngine;

namespace __ProjectExclusive.Player.UI
{
    // There's a need for knowing there's an UI for the player during combat.
    // This monoBehaviour will inject itself on the player's Singleton making sure that the GameObject exits
    public class UCombatUIInjector : MonoBehaviour
    {
        private void Awake()
        {
            if(PlayerCombatSingleton.CombatUiReference != null)
                throw new OverflowException("There's more than one Player Combat UI");

            PlayerCombatSingleton.CombatUiReference = this.gameObject;
        }
    }
}
