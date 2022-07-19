using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace CombatSystem.Player.UI.Info
{
    public class UPassivePopUp : MonoBehaviour
    {
        [Title("Texts")]
        [SerializeField] private TextMeshProUGUI highLightTextHolder;
        [SerializeField] private TextMeshProUGUI effectTextHolder;
        [Title("Images")]
        [SerializeField] private Image backgroundElement;
        [SerializeField] private Image passiveIcon;

        private TrackedMonoObjectPool<UPassivePopUp> _pool;


        public void Injection(TrackedMonoObjectPool<UPassivePopUp> pool) => _pool = pool;
        public void InjectHighLightText(string highLight)
        {
            highLightTextHolder.text = highLight;
        }
        public void InjectEffectText(string effectText)
        {
            effectTextHolder.text = effectText;
        }

        public void ChangeBackgroundColor(Color color)
        {
            backgroundElement.color = color;
            passiveIcon.color = color;
            effectTextHolder.color = color;
        }

        private void OnEnable()
        {
            _timerValue = 0;
        }

        private const float TimeThreshold = 1.5f;
        private float _timerValue;
        private void Update()
        {
            _timerValue += Time.deltaTime;
            if(_timerValue < TimeThreshold) return;

            gameObject.SetActive(false);
            _pool.Release(this);
        }
    }
}
