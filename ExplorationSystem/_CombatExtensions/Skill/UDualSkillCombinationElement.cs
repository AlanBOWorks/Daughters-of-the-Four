using System;
using System.Collections.Generic;
using CombatSystem.Localization;
using CombatSystem.Player.UI;
using CombatSystem.Skills;
using ExplorationSystem._CombatExtensions;
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
        [SerializeField] private UAllEffectTooltipsHandler effectsHolder;

        [Title("Skill params")]
        [SerializeField] private TextMeshProUGUI costHolder;
        [SerializeField] private TextMeshProUGUI luckHolder;
        [SerializeField] private GameObject ignoreSelfHolder;


        [Title("Data")]
        [ShowInInspector,EnableIf("_currentKey")]
        private IEffectsHolder _currentKey;

        private void Start()
        {
            ClearAndPrint();
        }

        private void OnDisable()
        {
            ClearAndPrint();
        }

        [Button,DisableInEditorMode]
        private void TestInjection(SSkillPresetBase skill) => DoInjection(skill);
        [Button,DisableInEditorMode]
        private void TestInjection(SDualSkillMaterial material) => DoInjection(material);

        public void DoInjection(IFullSkill skill) => DoInjection(skill, skill);
        public void DoInjection(IDualSkillMaterial material) => DoInjection(material, material);
        public void DoInjection(IEffectsHolder key, ISkillInfoHolder infoHolder)
        {
            if(_currentKey == key) return;

            _currentKey = key;

            effectsHolder.Clear();
            var nameData = infoHolder.GetSkillName();
            var icon = infoHolder.GetSkillIcon();
            HandleName(nameData);
            HandleIcon(icon);

            var effects = key.GetEffects();
            ISkill effectsKey;
            if (key is ISkill skill)
            {
                HandleAsSkill();
            }
            else
            {
                HandleJustEffects();
            }
            HandleEffects(effects, effectsKey);


            void HandleAsSkill()
            {
                effectsKey = skill;
                HandleExtraData(skill.SkillCost,skill.LuckModifier,skill.IgnoreSelf);
            }
            void HandleJustEffects()
            {
                effectsKey = null;
                // todo hideExtraData
            }
        }

        public void HideExtraData()
        {
            var extraDataRoot = costHolder.GetComponentInParent<GameObject>();
            extraDataRoot.SetActive(false);
        }


        private void HandleName(string skillName)
        {
            nameHolder.text = LocalizationsCombat.LocalizeSkillName(skillName);
        }

        private void HandleIcon(Sprite icon)
        {
            iconHolder.sprite = icon;
        }

        private void HandleExtraData(int skillCost, float luckModifier, bool isIgnoreSelf)
        {
            costHolder.text = skillCost.ToString();
            luckHolder.text = LocalizationsCombat.LocalizeLuck(luckModifier);

            ignoreSelfHolder.SetActive(isIgnoreSelf);
        }

        private void HandleEffects(IEnumerable<PerformEffectValues> effects,ISkill skill)
        {
            effectsHolder.HandleEffects(effects,skill);
        }

        private const string NullName = "---";
        private const string SingleDigitText = "-";
        public void ClearAndPrint()
        {
            _currentKey = null;

            nameHolder.text = NullName;
            iconHolder.sprite = null;
            costHolder.text = SingleDigitText;
            luckHolder.text = SingleDigitText;
            ignoreSelfHolder.SetActive(false);
            effectsHolder.Clear();
        }
    }
}
