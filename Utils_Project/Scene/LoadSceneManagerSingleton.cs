using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils_Project.Scene
{
    internal static class LoadSceneManagerSingleton
    {
        public static ULoadSceneManager ManagerInstance { get; private set; }
        public static void Injection(ULoadSceneManager manager)
        {
            var currentInstance = ManagerInstance;
            if (currentInstance) Object.Destroy(currentInstance.transform.gameObject);

            ManagerInstance = manager;
        }
    }


    public interface ILoadSceneManagerStructureRead<out T>
    {
        T MainLoadType { get; }
        T CombatLoadType { get; }
    }

    public interface ILoadSceneAnimator
    {
        void SetActive(bool active);

        IEnumerator<float> _DoInitialAnimation();
        void TickingLoad(float currentPercent);
        IEnumerator<float> _OnAfterLoadAnimation();
    }
}
