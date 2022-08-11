using System;
using MPUIKIT;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MainMenu
{
    public class UMainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
         IPointerClickHandler
    {
        [SerializeField] private MPImage backgroundImage;
        [SerializeField] private TextMeshProUGUI textHolder;



        private static readonly Color OnExitColor = new Color(.05f, .05f, .05f);
        private static readonly Color OnHoverColor = new Color(.94f, .94f, .94f);
        private const float HoverDeltaSpeed = 4f;
        private const float ClickDeltaSpeed = 8f;


        [ShowInInspector, HideInEditorMode]
        private float _lerpAmount;
        private void LateUpdate()
        {
            if (_hoverEnter) 
                TickOnHover();
            TickClick();
        }

        private void TickOnHover()
        {
            if(_lerpAmount >= 1) return;

            float deltaVariation = Time.deltaTime * HoverDeltaSpeed;
            TickColors(deltaVariation);
        }

      
        private void TickColors(float deltaVariation)
        {
            _lerpAmount += deltaVariation;
            _lerpAmount = Mathf.Clamp01(_lerpAmount);

            textHolder.color = Color.LerpUnclamped(textHolder.color, OnHoverColor, _lerpAmount);

            backgroundImage.fillAmount = Mathf.LerpUnclamped(backgroundImage.fillAmount, 1, _lerpAmount);
        }

        private void TickClick()
        {
            var textTransform = textHolder.transform;
            Vector3 localScale = textTransform.localScale;
            if(localScale.x >= 1) return;

            float deltaVariation = Time.deltaTime * ClickDeltaSpeed;
            textTransform.localScale = Vector3.Lerp(localScale, Vector3.one, deltaVariation);
        }


        private bool _hoverEnter;
        public void OnPointerEnter(PointerEventData eventData)
        {
            _hoverEnter = true;
            _lerpAmount += .01f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _hoverEnter = false;
            textHolder.color = OnExitColor;
            backgroundImage.fillAmount = 0;
            _lerpAmount = 0;
        }

        private const float OnClickScale = .85f;
        public void OnPointerClick(PointerEventData eventData)
        {
            textHolder.transform.localScale = new Vector3(OnClickScale,OnClickScale, 1);
        }
    }
}
