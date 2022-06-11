using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace CombatSystem.Player.UI
{
    public class URolesIconsSpawner : MonoBehaviour
    {
        [Title("Params")]
        [SerializeField, SuffixLabel("px")] 
        private Vector2 spawnDirectionPerIcon;
        [SerializeField,Tooltip("true: spawnDirectionPerIcon * i=0")] 
        private bool countIndexStartAtZero = true;
        [SerializeField] 
        private bool iconIsChild = true;

        [Title("Prefab")]
        [SerializeReference]
        private ImageHolderPrefab prefab = new ImageHolderPrefab();

        private void Start()
        {
            SpawnIcons();
        }

        private void LateUpdate()
        {
            var prefabGameObject = prefab.GetPrefab().gameObject;
            Destroy(prefabGameObject);
            Destroy(this);
        }

        private void SpawnIcons()
        {
            var rolesTheme = CombatThemeSingleton.RolesThemeHolder;

            int i = (countIndexStartAtZero) ? 0 : 1;
            foreach (var theme in rolesTheme.GetEnumerable())
            {
                var icon = theme.GetThemeIcon();
                HandleImage(i,icon);
                i++;
            }
        }

        private void HandleImage(int i, Sprite icon)
        {
            var imageHolder = prefab.SpawnElement();
            RectTransform holder = imageHolder.rectTransform;
            holder.localPosition = spawnDirectionPerIcon * i;

            if (iconIsChild)
            {
                // PROBLEM: both parent and child have [Image], thus making the
                // GetComponentInChild returning the self(parent) component instead
                // the child' one
                // SOLUTION: get child transform and get the component from there
                var child = holder.GetChild(0);
                var childImageHolder = child.GetComponent<Image>();

                HandleIcon(childImageHolder, icon);
            }
            else
                HandleIcon(imageHolder, icon);
        }

        private static void HandleIcon(Image holder, Sprite icon)
        {
            holder.sprite = icon;
        }


        [Serializable]
        private sealed class ImageHolderPrefab : PrefabInstantiationHandler<Image>
        {
            
        }
    }
}
