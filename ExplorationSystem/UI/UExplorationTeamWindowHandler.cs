using System;
using System.Collections;
using CombatSystem.Entity;
using CombatSystem.Player.UI;
using CombatSystem.Team;
using ExplorationSystem.Team;
using UnityEngine;

namespace ExplorationSystem.UI
{
    public class UExplorationTeamWindowHandler : MonoBehaviour
    {
        [SerializeField] 
        private UExplorationSkillsWindowHandler skillsWindow;
        [SerializeField] 
        private SpawnReferences spawner = new SpawnReferences();

        private void Awake()
        {
            spawner.DoInstantiations();
            HandleIcons();
        }

        private void OnEnable()
        {
            var team = PlayerExplorationSingleton.GetCurrentSelectedTeam();
            if(team == null) return;

            InjectDataValues(team);
        }

#if UNITY_EDITOR
        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            HandleTeam();
        }
#endif
        private void HandleTeam()
        {
            var team = PlayerExplorationSingleton.GetCurrentSelectedTeam();
            if (team == null) return;

            InjectDataValues(team);
        }


        private ICombatEntityProvider _currentEntity;
        public void ShowSkillList(ICombatEntityProvider entity)
        {
            if (_currentEntity != null && entity == _currentEntity)
            {
                _currentEntity = null;
                HideSkillList();
                return;
            }
            _currentEntity = entity;
            skillsWindow.SwitchEntity(entity);
        }

        public void HideSkillList()
        {
            skillsWindow.Hide();
        }


        private const float MarginHeightSeparation = 36;
        private void HandleIcons()
        {
            ITeamFlexStructureRead<CombatThemeHolder> themeHolder 
                = CombatThemeSingleton.RolesThemeHolder;
            var enumerable = UtilsTeam.GetEnumerable(spawner, themeHolder);

            var prefab = spawner.GetPrefabElement();
            var prefabTransform = (RectTransform) prefab.transform;
            var elementHeight = prefabTransform.rect.height;

            int i = EnumTeam.RoleTypesCount -1;
            foreach ((UExplorationTeamMemberElement component, CombatThemeHolder theme) in enumerable)
            {
                var componentTransform = (RectTransform) component.transform;

                var roleIconHolder = component.GetComponent<URoleIconHolder>();
                roleIconHolder.iconHolder.sprite = theme.GetThemeIcon();

                float heightPosition = i * (elementHeight + MarginHeightSeparation);
                Vector2 localPosition = new Vector2(0, heightPosition);

                componentTransform.anchoredPosition = localPosition;
                i--;
            }
        }


        public void InjectDataValues(ITeamFlexStructureRead<PlayerRunTimeEntity> team)
        {
            var enumerable = UtilsTeam.GetEnumerable(team, spawner);

            foreach ((PlayerRunTimeEntity member, UExplorationTeamMemberElement memberElement) in enumerable)
            {
                memberElement.Injection(this);
                memberElement.Injection(member);
                memberElement.UpdateHealth();
            }
        }
       

        [Serializable]
        private sealed class SpawnReferences : TeamPrefabElementsSpawner<ICombatEntityProvider,UExplorationTeamMemberElement>
        {
            
        }
    }
}
