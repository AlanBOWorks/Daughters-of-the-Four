using System;
using System.Globalization;
using ___ProjectExclusive;
using _CombatSystem;
using _Player;
using Characters;
using Sirenix.OdinInspector;
using Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Player
{
    public class UCharacterUIFixedHolder : UCharacterUIHolderBase
    {
        [Title("Character")]
        [SerializeField] private TextMeshProUGUI characterName = null;

        [Title("Combat Stats")] 
        [SerializeField] private RoleTooltip roleTooltip = new RoleTooltip();
        [SerializeField] private HarmonyTooltip harmonyTooltip = new HarmonyTooltip();

        [Title("Handlers")]
        [SerializeField] private UTargetButton buttonHandler;

        public override void Injection(CombatingEntity entity)
        {
            characterName.text = entity.CharacterName;

            base.Injection(entity);
            roleTooltip.Injection(entity);
            buttonHandler.Injection(entity);
            harmonyTooltip.Injection(entity);
        }

        public void ShowTargetButton() => buttonHandler.Show();
        public void HideTargetButton() => buttonHandler.Hide();
    }

    public abstract class UCharacterUIHolderBase : MonoBehaviour
    {
        [SerializeField] private HealthTooltip healthTooltip = new HealthTooltip();

        public virtual void Injection(CombatingEntity entity)
        {
            healthTooltip.Injection(entity);
        }
    }

    [Serializable]
    internal class RoleTooltip
    {
        [SerializeField] private Image roleIcon;

        public void Injection(CombatingEntity entity)
        {
            var icons = GameThemeSingleton.ThemeVariable.RoleIcons;
            Sprite roleSprite = UtilsCharacter.GetElement(icons, entity.Role);
            roleIcon.sprite = roleSprite;
        }
    }

    [Serializable]
    internal class HealthTooltip : IVitalityChangeListener, ICombatHealthChangeListener
    {
        [SerializeField] private TextMeshProUGUI healthAmount = null;
        [SerializeField] private TextMeshProUGUI maxHealth = null;

        public void Injection(CombatingEntity entity)
        {
            var stats = entity.CombatStats;

            OnVitalityChange(stats);
            OnTemporalStatsChange(stats.BaseStats);

            entity.Events.Subscribe(this);
        }


        public void OnVitalityChange(IVitalityStatsData<float> currentStats)
        {

            maxHealth.text = UtilsGameTheme.GetNumericalPrint(currentStats.MaxHealth);
        }

        public void OnTemporalStatsChange(ICombatHealthStatsData<float> currentStats)
        {
            healthAmount.text = UtilsGameTheme.GetNumericalPrint(currentStats.HealthPoints);
        }
    }

    [Serializable]
    internal class HarmonyTooltip : ITemporalStatChangeListener
    {
        [SerializeField] private TextMeshProUGUI harmonyText;

        public void Injection(CombatingEntity entity)
        {
            OnConcentrationChange(entity.CombatStats);

            entity.Events.Subscribe(this);
        }

        public void OnConcentrationChange(ITemporalStatsData<float> currentStats)
        {
            float tooltipHarmony = 100 * currentStats.HarmonyAmount;
            harmonyText.text = UtilsGameTheme.GetPercentagePrint(tooltipHarmony);
        }
    }

}
