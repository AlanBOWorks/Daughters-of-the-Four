using System;
using CombatSystem.Entity;
using Sirenix.OdinInspector;
using TMPro;
using UltEvents;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExplorationSystem.Elements
{
    public class UExplorationCharacterSelectorElement : MonoBehaviour, IPointerClickHandler
    {
        [TitleGroup("References")]
        [SerializeField] 
        private Image iconHolder;

        public delegate void OnSelectionDelegate(ICombatEntityProvider user);
        public event OnSelectionDelegate OnClickEvent;

        public ICombatEntityProvider CurrentUser { get; private set; }

        public void Injection(ICombatEntityProvider user)
        {
            CurrentUser = user;
        }

        public void Injection(Sprite icon)
        {
            iconHolder.sprite = icon;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (CurrentUser == null)
            {
                throw new NullReferenceException("Invoked [Character Selection] with a _Null_ entity;\n " +
                                                 "The Injection could had failed or something might had set it to _Null_");
            }
            OnClickEvent?.Invoke(CurrentUser);
        }
    }
}
