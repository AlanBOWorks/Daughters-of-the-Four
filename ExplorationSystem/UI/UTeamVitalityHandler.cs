using System;
using CombatSystem.Entity;
using CombatSystem.Player.UI;
using CombatSystem.Team;
using ExplorationSystem.Team;
using UnityEngine;

namespace ExplorationSystem.UI
{
    public class UTeamVitalityHandler : MonoBehaviour
    {
        [SerializeField] private SpawnReferences spawner = new SpawnReferences();

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
            foreach ((UHealthInfo component, CombatThemeHolder theme) in enumerable)
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

            foreach ((PlayerRunTimeEntity member, UHealthInfo healthHolder) in enumerable)
            {
                healthHolder.UpdateHealth(member,member);
            }
        }
       

        [Serializable]
        private sealed class SpawnReferences : TeamPrefabElementsSpawner<ICombatEntityProvider,UHealthInfo>
        {
            
        }
    }
}
