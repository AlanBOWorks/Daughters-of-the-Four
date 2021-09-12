using System;
using _CombatSystem;
using Characters;
using DG.Tweening;
using Sirenix.OdinInspector;
using Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Player
{
    public class UTempoTooltip : MonoBehaviour, IPersistentElementInjector, ITempoListener, ITempoFiller
    {
        [SerializeField] private TempoBarFiller tempoFiller = new TempoBarFiller();
        [SerializeField] private TempoActionTooltip actionTooltip = new TempoActionTooltip();

        public void DoInjection(EntityPersistentElements persistentElements)
        {
            persistentElements.CombatEvents.Subscribe(this);
            persistentElements.TempoFillers.Add(this);
        }

        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            actionTooltip.OnInitiativeTrigger(entity);
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
            actionTooltip.OnDoMoreActions(entity);
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            actionTooltip.OnFinisAllActions(entity);
        }

        public void FillBar(float percentage)
        {
            tempoFiller.FillBar(percentage);
        }
    }

    [Serializable]
    public class TempoActionTooltip : ITempoListener
    {
        [SerializeField] private TextMeshProUGUI actionText;
        [SerializeField] private Image activeIcon;

        private void UpdateAmountOfActions(CombatingEntity entity)
        {
            int actionsLeft = entity.CombatStats.ActionsLefts;
            UpdateAmountOfActions(actionsLeft);
        }

        private void UpdateAmountOfActions(int actionsLeft)
        {
            actionText.text = UtilStringStats.GetStaticString(actionsLeft);

        }

        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            UpdateAmountOfActions(entity);
            activeIcon.gameObject.SetActive(true);
            Vector3 rotationPunch = new Vector3(0,0,60);
            activeIcon.rectTransform.DOPunchRotation(rotationPunch, .4f);

        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
            UpdateAmountOfActions(entity);
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            UpdateAmountOfActions(0);
            activeIcon.gameObject.SetActive(false);
        }
    }


    [Serializable]
    public class TempoBarFiller : ITempoFiller
    {
        [Title("UI")]
        [SerializeField]
        private RectTransform fillerTransform = null;

        [Title("Params")]
        [SerializeField, Range(-5f, 5f)] private float percentageModifier = 1f;
        public void FillBar(float percentage)
        {
            fillerTransform.DOPivotX(percentage * percentageModifier, TempoHandler.DeltaStepPeriod);
        }

    }
}
