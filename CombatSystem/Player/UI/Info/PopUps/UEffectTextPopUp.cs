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

        private float _lerpAmount;
        public float GetCurrentLerp() => _lerpAmount;

        private void Awake()
        {
            _alphaGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            _lerpAmount = 0;
        }

        private void LateUpdate()
        {
            if (_lerpAmount < 1) return;

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

        public void Injection(Sprite effectSprite)
        {
            effectIcon.sprite = effectSprite;
        }

        public void Translation(Vector2 translation)
        {
            transform.Translate(translation);
        }

        public void SetAlpha(float alpha)
        {
            _alphaGroup.alpha = alpha;
        }
    }
}
