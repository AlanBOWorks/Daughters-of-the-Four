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

        public override void OnEntitySwitch(CombatingEntity entity)
        {
            base.OnEntitySwitch(entity);
            entity.Events.Subscribe(this);
        }

        protected override Vector3 GetUIPosition(UCharacterHolder holder)
        {
            return holder.meshTransform.position;
        }

        public void OnAreaStateChange(CharacterCombatAreasData data)
        {
            stanceTooltip.UpdateStance(data.GetCurrentPositionState());
        }
    }

    [Serializable]
    internal class CharacterStanceTooltip : IStanceData<string>
    {
        [SerializeField] private TextMeshProUGUI stanceText;

        private const string AttackingToolTip = "[ Attaking ]";
        private const string NeutralTooltip = "[ Neutral ]";
        private const string DefendingTooltip = "[ Defending ]";
        public void UpdateStance(EnumTeam.Stances targetStance)
        {
            var tooltip=
                UtilsTeam.GetElement(this, targetStance);
            stanceText.text = tooltip;
        }

        public string AttackingStance => AttackingToolTip;

        public string NeutralStance => NeutralTooltip;

        public string DefendingStance => DefendingTooltip;
    }

}
