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

        public void Injection(Sprite effectSprite)
        {
            effectIcon.sprite = effectSprite;
        }

        public void Translation(Vector2 translation)
        {
            Vector2 lerpedTranslation = (1 - LerpAmount) * translation;
            transform.Translate(lerpedTranslation);
        }

        public void SetAlpha(float alpha)
        {
            _alphaGroup.alpha = alpha;
        }
    }
}
