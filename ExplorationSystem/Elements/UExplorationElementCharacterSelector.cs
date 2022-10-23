using System;
using CombatSystem;
using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Player.UI;
using CombatSystem.Team;
using ExplorationSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace ExplorationSystem.Elements
{
    public class UExplorationElementCharacterSelector : MonoBehaviour
    {
        [SerializeField]
        private SpawnReferences spawner = new SpawnReferences();

        private void Awake()
        {
            spawner.DoInstantiations();
            spawner.RePositionElements();
        }



        [Serializable]
        private sealed class SpawnReferences : TeamPrefabElementsSpawner<ICombatEntityProvider, UExplorationCharacterSelectorElement>
        {
            [SerializeField, SuffixLabel("px")] private float lateralMargin = 12;


            public void RePositionElements()
            {
                ITeamFlexStructureRead<CombatThemeHolder> themeHolder = CombatThemeSingleton.RolesThemeHolder;
                var enumerable = UtilsTeam.GetEnumerable(this, themeHolder);

                var prefab = GetPrefabElement();
                var prefabTransform = (RectTransform) prefab.transform;
                var elementWidth = prefabTransform.rect.width;

                float i = -1.5f; // There are 4 elements
                foreach ((UExplorationCharacterSelectorElement element, CombatThemeHolder theme) in enumerable)
                {
                    element.Injection(theme.GetThemeIcon());

                    var elementTransform = (RectTransform) element.transform;
                    var position = elementTransform.anchoredPosition;
                    position.x = i * (elementWidth + lateralMargin);
                    elementTransform.anchoredPosition = position;
                    i++;
                }
            }
        }
    }
}
