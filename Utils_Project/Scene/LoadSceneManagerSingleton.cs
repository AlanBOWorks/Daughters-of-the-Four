using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils_Project.Scene
{
    internal static class LoadSceneManagerSingleton
    {
        private static readonly List<IScreenLoadListener> ScreenLoadListenersCollection;
        private static readonly List<ILoadPercentListener> LoadPercentListenersCollection;

        static LoadSceneManagerSingleton()
        {
            ScreenLoadListenersCollection = new List<IScreenLoadListener>();
            LoadPercentListenersCollection = new List<ILoadPercentListener>();
        }

        public static ICollection<IScreenLoadListener> ScreenLoadListeners => ScreenLoadListenersCollection;
        public static ICollection<ILoadPercentListener> LoadPercentListeners => LoadPercentListenersCollection;

        public static ULoadSceneManager ManagerInstance { get; private set; }
        public static void Injection(ULoadSceneManager manager)
        {
            var currentInstance = ManagerInstance;
            if (currentInstance) Object.Destroy(currentInstance);

            ManagerInstance = manager;
        }
    }

    /// <summary>
    /// Listeners in key Step during load screens
    /// </summary>
    public interface IScreenLoadListener
    {
        void OnShowLoadScreen(LoadSceneParameters.LoadType type);
        void OnHideLoadScreen(LoadSceneParameters.LoadType type);
        void OnFillLoadScreenPercent(float fillPercent);
        void OnFillOutLoadScreenPercent(float fillPercent);
    }

    /// <summary>
    /// Listeners with ticking feedback 
    /// </summary>
    public interface ILoadPercentListener
    {
        void OnPercentTick(float loadPercent);
    }



    public interface ILoadSceneManagerStructureRead<out T>
    {
        T MainLoadType { get; }
        T CombatLoadType { get; }
    }
}
