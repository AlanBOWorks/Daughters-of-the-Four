using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CombatSystem
{
    public class UCombatSceneHelper : MonoBehaviour
    {
        [SerializeField] private GameObject[] toggleableObjects;
        [SerializeField] private bool isCombatScene;

        public GameObject[] ToggleableObjects => toggleableObjects;
        public bool IsCombatScene => isCombatScene;

        private void Awake()
        {
            CombatSystemSingleton.SceneTracker.HandleCombatScenes(this);
        }

        private void OnDisable()
        {
            foreach (GameObject disableObject in toggleableObjects)
            {
                disableObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            foreach (GameObject enableObject in toggleableObjects)
            {
                enableObject.SetActive(true);
            }
        }
    }

    public class CombatSceneTracker 
    {
        public UCombatSceneHelper MainSceneHelper => _mainSceneHelper;
        public Scene MainSceneData => _mainSceneData;
        public UCombatSceneHelper CombatSceneHelper => _combatSceneHelper;
        public Scene CombatSceneData => _combatSceneData;

        private UCombatSceneHelper _mainSceneHelper;
        private Scene _mainSceneData;
        private UCombatSceneHelper _combatSceneHelper;
        private Scene _combatSceneData;

        public void HandleCombatScenes(UCombatSceneHelper helper)
        {
            if (helper.IsCombatScene)
            {
                Injection(ref _mainSceneHelper, ref _mainSceneData);
            }
            else
            {
                Injection(ref _combatSceneHelper, ref _combatSceneData);
            }

            void Injection(ref UCombatSceneHelper holder, ref Scene scene)
            {
                var holderScene = helper.gameObject.scene;

                holder = helper;
                scene = holderScene;
            }
        }

    }
}
