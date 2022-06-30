using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils.UI
{
    public class UButtonPointerFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [TitleGroup("FeedBack")]
        [SerializeField, HideInPlayMode]
        private RectTransform targetButton;
        [TitleGroup("FeedBack")]
        [SerializeReference]
        private IButtonPointerFeedbackHandler handler;
        

        private void Awake()
        {
            handler.Injection(targetButton);
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            handler.OnPointerEnter(eventData);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            handler.OnPointerExit(eventData);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            handler.OnPointerClick(eventData);
        }
    }

    public interface IButtonPointerFeedbackHandler 
    {
        void Injection(RectTransform button);
        void OnPointerEnter(PointerEventData eventData);
        void OnPointerExit(PointerEventData eventData);
        void OnPointerClick(PointerEventData eventData);
    }
}
