using System;
using CombatSystem.Localization;
using CombatSystem.Player.UI;
using CombatSystem.Skills;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExplorationSystem.UI
{
    public class UDualSkillCombinationElement : MonoBehaviour
    {
        [Title("References")] 
        [SerializeField] private TextMeshProUGUI nameHolder;
        [SerializeField] private Image iconHolder;
        [SerializeField] private TextMeshProUGUI costHolder;
        [SerializeField] private TextMeshProUGUI luckHolder;
        [SerializeField] private GameObject ignoreSelfHolder;
        [SerializeField] private UAllEffectTooltipsHandler effectsHolder;


        [Title("Data")]
        [ShowInInspector,EnableIf("_currentSkill")]
        private IFullSkill _currentSkill;

        private void Start()
        {
            ClearAndPrint();
        }

        private void OnDisable()
        {
            ClearAndPrint();
        }

        [Button,DisableInEditorMode]
        private void TestInjection(SSkillPresetBase skill) => Injection(skill);
        public void Injection(IFullSkill skill)
        {
            if(_currentSkill == skill) return;

            effectsHolder.Clear();

            _currentSkill = skill;
            HandleName();
            HandleIcon();
            HandleExtraData();
            HandleEffects();
        }

        private void HandleName()
        {
            var skillName = _currentSkill.GetSkillName();
            nameHolder.text = LocalizationsCombat.LocalizeSkillName(skillName);
        }

        private void HandleIcon()
        {
            var icon = _currentSkill.GetSkillIcon();
            iconHolder.sprite = icon;
        }

        private void HandleExtraData()
        {
            costHolder.text = _currentSkill.SkillCost.ToString();
            luckHolder.text = LocalizationsCombat.LocalizeLuck(_currentSkill.LuckModifier);

            bool isIgnoreSelf = _currentSkill.IgnoreSelf;
            ignoreSelfHolder.SetActive(isIgnoreSelf);
        }

        private void HandleEffects()
        {
            effectsHolder.HandleEffects(_currentSkill.GetEffectsFeedBacks(),_currentSkill);
        }

        private const string NullName = "---";
        private const string SingleDigitText = "-";
        public void ClearAndPrint()
        {
            _currentSkill = null;

            nameHolder.text = NullName;
            iconHolder.sprite = null;
            costHolder.text = SingleDigitText;
            luckHolder.text = SingleDigitText;
            ignoreSelfHolder.SetActive(false);
            effectsHolder.Clear();
        }
    }
}
