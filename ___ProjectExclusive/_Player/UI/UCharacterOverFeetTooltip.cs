using System;
using _CombatSystem;
using _Team;
using Characters;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace _Player
{
    public class UCharacterOverFeetTooltip : UCharacterOverTooltipBase, IAreaStateChangeListener
    {
        [SerializeField] 
        private CharacterStanceTooltip stanceTooltip = new CharacterStanceTooltip();

        public override void Injection(CombatingEntity entity, bool isPlayer)
        {
            base.Injection(entity, isPlayer);
            entity.Events.Subscribe(this);

            OnAreaStateChange(entity.AreasDataTracker);
        }

        protected override Vector3 GetUIPosition(UCharacterHolder holder)
        {
            return holder.meshTransform.position;
        }

        public void OnAreaStateChange(CombatAreasData data)
        {
            stanceTooltip.UpdateStance(data.PositionStance);
        }
    }

    [Serializable]
    internal class CharacterStanceTooltip : IStanceArchetype<string>
    {
        [SerializeField] private TextMeshProUGUI stanceText;

        private const string AttackingToolTip = "[ Attaking ]";
        private const string NeutralTooltip = "[ Neutral ]";
        private const string DefendingTooltip = "[ Defending ]";
        public void UpdateStance(TeamCombatData.Stance targetStance)
        {
            var tooltip=
                TeamCombatData.GetStance(this, targetStance);
            stanceText.text = tooltip;
        }

        public string GetAttacking() => AttackingToolTip;

        public string GetNeutral() => NeutralTooltip;

        public string GetDefending() => DefendingTooltip;
    }

}
