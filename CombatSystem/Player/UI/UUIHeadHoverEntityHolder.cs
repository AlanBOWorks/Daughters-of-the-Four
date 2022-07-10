using System;
using CombatSystem.Entity;
using CombatSystem.Skills.Effects;
using CombatSystem.Stats;
using SCharacterCreator.Bones;
using TMPro;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UUIHeadHoverEntityHolder : MonoBehaviour
    {
        [SerializeField]
        private ActionsInfoHandler actionsInfoHandler;
        private Transform _followReference;

        private RectTransform _rectTransform;

        public void Injection(CombatEntity entity)
        {
            _followReference = entity.Body.HeadRootType;
            UpdateActions(entity.Stats);
        }

        private void Awake()
        {
            _rectTransform = (RectTransform)transform;
        }
        private void LateUpdate()
        {
            var targetPoint = (GetPointFollow());
            _rectTransform.position = targetPoint;
            var localPosition = _rectTransform.localPosition;
            localPosition.z = 0;
            _rectTransform.localPosition = localPosition;
        }
        private Vector3 GetPointFollow()
        {
            return _followReference.position;
        }


        public void UpdateActions(CombatStats stats)
        {
            float speed = UtilsStatsFormula.CalculateInitiativeSpeed(stats);
            actionsInfoHandler.UpdateActionAmount(speed);
        }


        [Serializable]
        private struct ActionsInfoHandler
        {
            [SerializeField] private TextMeshProUGUI actionsTextHolder;

            public void UpdateActionAmount(float speedAmount)
            {
                string speedText = speedAmount.ToString("##");
                actionsTextHolder.text = speedText;
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}
