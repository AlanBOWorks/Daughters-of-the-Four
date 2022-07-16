using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace CombatSystem.Player.UI
{
    public class UEffectTextPopUp : MonoBehaviour
    {
        private CanvasGroup _alphaGroup;
        [SerializeField] private TextMeshProUGUI effectTextHolder;
        [SerializeField] private Image effectIcon;

        private TrackedMonoObjectPool<UEffectTextPopUp> _pool;

        [NonSerialized]
        public float LerpAmount;

        private Vector3 _initialPoint;
        private Vector3 _targetPoint;



        private void Awake()
        {
            _alphaGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            LerpAmount = 0;
        }

        private void LateUpdate()
        {
            if (LerpAmount < 1) return;

            gameObject.SetActive(false);
            _pool.Release(this);
        }

        public void Injection(TrackedMonoObjectPool<UEffectTextPopUp> pool)
        {
            _pool = pool;
        }

        public void Injection(string effectText)
        {
            effectTextHolder.text = effectText;
        }

        public void SetDestination(Vector3 offsetPoint)
        {
            _initialPoint = transform.localPosition;
            _targetPoint = _initialPoint + offsetPoint;
        }

        public void Injection(Sprite effectSprite)
        {
            effectIcon.sprite = effectSprite;
        }

        public void SetAlpha(float alpha)
        {
            _alphaGroup.alpha = alpha;
        }

        public void Move(float destinationLerp)
        {
            transform.localPosition = Vector3.LerpUnclamped(_initialPoint, _targetPoint, destinationLerp);
        }
    }
}
