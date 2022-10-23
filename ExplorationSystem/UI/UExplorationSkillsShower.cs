using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Team;
using CombatSystem.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

namespace ExplorationSystem.UI
{
    public class UExplorationSkillsShower : MonoBehaviour, USkillElementHolder.ISkillElementEventsHandler
    {
        [SerializeReference]
        private Spawner skillListSpawner = new Spawner();
        [SerializeReference]
        private Spawner divineSkillsSpawner = new Spawner();

        private void Awake()
        {
            skillListSpawner.Awake();
            skillListSpawner.EventsHandler = this;

            _stancesRecord = new Dictionary<ICombatEntityProvider, EnumTeam.Stance>();
        }

        private void OnDisable()
        {
            CurrentEntity = null;
            skillListSpawner.Clear();
            _stancesRecord = new Dictionary<ICombatEntityProvider, EnumTeam.Stance>();
        }

        private Dictionary<ICombatEntityProvider, EnumTeam.Stance> _stancesRecord;

        public ICombatEntityProvider CurrentEntity { get; private set; }

        private EnumTeam.Stance GetTargetStanceOnEntity(ICombatEntityProvider entity)
        {
            if (_stancesRecord.ContainsKey(entity)) return _stancesRecord[entity];
            var targetStance = UtilsTeam.ParseStance(entity.GetAreaData().RoleType);
            _stancesRecord.Add(entity,targetStance);
            return targetStance;
        }

        public void HandleEntity(ICombatEntityProvider entityProvider)
        {
            CurrentEntity = entityProvider;
            var stance = GetTargetStanceOnEntity(entityProvider);
            var skills = UtilsTeam.GetElement(stance, entityProvider.GetPresetSkills());
            skillListSpawner.HandleSkills(skills);
            HandleDivineSkillsHolderPosition();
        }

        private void HandleDivineSkillsHolderPosition()
        {
            var skillsListSeparation = skillListSpawner.TotalSeparation;
            var divineSkillsRoot = (RectTransform) divineSkillsSpawner.ElementsHolder.parent;
            var position = divineSkillsRoot.anchoredPosition;
            position.x = skillsListSeparation;
            divineSkillsRoot.anchoredPosition = position;
        }

        public void OnPointerEnter(IFullSkill skill)
        {
        }

        public void OnPointerExit(IFullSkill skill)
        {
        }

        public void OnPointerClick(IFullSkill skill)
        {
        }



        [Button,DisableInEditorMode]
        private void TestShowSkill(SPlayerPreparationEntity entity)
        {
            skillListSpawner.Clear();
            HandleEntity(entity);
        }

        [Serializable]
        private sealed class Spawner : DictionaryPool<IFullSkill, USkillElementHolder>
        {
            [TitleGroup("References")]
            [SerializeField] private RectTransform onPoolParent;

            [TitleGroup("References")] 
            [SerializeField]
            private RectTransform onReleaseParent;

            [TitleGroup("Params")] 
            [SerializeField, SuffixLabel("px"),DisableInPlayMode]
            private float lateralSeparation;


            public RectTransform ElementsHolder => onPoolParent;

            private float _finalElementSeparation;
            public USkillElementHolder.ISkillElementEventsHandler EventsHandler { private get; set; }
            public override void Awake()
            {
                base.Awake();
                onReleaseParent.gameObject.SetActive(false);
                var prefabTransform =  (RectTransform) GetPrefab().transform;
                _finalElementSeparation = prefabTransform.sizeDelta.x + lateralSeparation;
            }

            public float TotalSeparation { get; private set; }
            public void HandleSkills(IEnumerable<IFullSkill> skills)
            {
                foreach (var skill in skills)
                {
                    Pop(skill);
                }

                TotalSeparation = (Dictionary.Count + 1) * _finalElementSeparation;
            }



            public override USkillElementHolder Pop(IFullSkill key)
            {
                var element = base.Pop(key);
                var elementTransform = (RectTransform) element.transform;
                elementTransform.SetParent(onPoolParent);
                element.Injection(key);
                element.Injection(EventsHandler);

                int i = Dictionary.Count -1;
                var position = elementTransform.anchoredPosition;
                position.x = i * _finalElementSeparation;
                elementTransform.anchoredPosition = position;

                return element;
            }

            protected override void OnRelease(IFullSkill key, USkillElementHolder element)
            {
                base.OnRelease(key, element);
                element.transform.SetParent(onReleaseParent);
            }
        }

    }
}
