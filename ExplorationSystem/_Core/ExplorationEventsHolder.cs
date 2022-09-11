using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ExplorationSystem
{
    public sealed class ExplorationEventsHolder : IExplorationEventsHolder
    {
        public ExplorationEventsHolder()
        {
            _worldSceneListeners = new HashSet<IWorldSceneListener>();
            _sceneChangeListeners = new HashSet<ISceneChangeListener>();
            _explorationSubmitListeners = new HashSet<IExplorationSubmitListener>();
            _onCombatListeners = new HashSet<IExplorationOnCombatListener>();
        }

        [ShowInInspector,HorizontalGroup("Scene Listeners")] 
        private readonly HashSet<IWorldSceneListener> _worldSceneListeners;
        [ShowInInspector,HorizontalGroup("Scene Listeners")] 
        private readonly HashSet<ISceneChangeListener> _sceneChangeListeners;

        [ShowInInspector,HorizontalGroup("Element Listeners")] 
        private readonly HashSet<IExplorationSubmitListener> _explorationSubmitListeners;
        [ShowInInspector,HorizontalGroup("Element Listeners")] 
        private readonly HashSet<IExplorationOnCombatListener> _onCombatListeners;
       

        public void Subscribe(IExplorationEventListener listener)
        {
            if (listener is ISceneChangeListener sceneChangeListener)
                _sceneChangeListeners.Add(sceneChangeListener);
            if (listener is IWorldSceneListener worldSceneListener)
                _worldSceneListeners.Add(worldSceneListener);

            if (listener is IExplorationSubmitListener submitListener)
                _explorationSubmitListeners.Add(submitListener);
            if (listener is IExplorationOnCombatListener combatListener)
                _onCombatListeners.Add(combatListener);
        }
        public void UnSubscribe(IExplorationEventListener listener)
        {
            if (listener is ISceneChangeListener sceneChangeListener)
                _sceneChangeListeners.Remove(sceneChangeListener);
            if (listener is IWorldSceneListener worldSceneListener)
                _worldSceneListeners.Remove(worldSceneListener);

            if (listener is IExplorationSubmitListener submitListener)
                _explorationSubmitListeners.Remove(submitListener);
            if (listener is IExplorationOnCombatListener combatListener)
                _onCombatListeners.Remove(combatListener);
        }

        public void OnWorldSelectSceneLoad(IExplorationSceneDataHolder sceneData)
        {
            foreach (var listener in _sceneChangeListeners) 
                listener.OnWorldSelectSceneLoad(sceneData);
        }

        public void OnWorldSceneOpen(IExplorationSceneDataHolder lastMap)
        {
            foreach (var listener in _worldSceneListeners) 
                listener.OnWorldSceneOpen(lastMap);
        }

        public void OnWorldMapClose(IExplorationSceneDataHolder targetMap)
        {
            foreach (var listener in _worldSceneListeners) 
                listener.OnWorldMapClose(targetMap);
        }

        public void OnExplorationRequest(EnumExploration.ExplorationType type)
        {
            foreach (var listener in _explorationSubmitListeners)
                listener.OnExplorationRequest(type);
        }

        public void OnExplorationCombatLoadFinish(EnumExploration.ExplorationType type)
        {
            foreach (var listener in _onCombatListeners)
                listener.OnExplorationCombatLoadFinish(type);
        }

        public void OnExplorationReturnFromCombat(EnumExploration.ExplorationType fromCombatType)
        {
            foreach (var listener in _onCombatListeners)
                listener.OnExplorationReturnFromCombat(fromCombatType);
        }
    }


    internal interface IExplorationEventsHolder : ISceneChangeListener, IWorldSceneListener,
        IExplorationSubmitListener,
        IExplorationOnCombatListener
    { }    
    public interface IExplorationEventListener { }


    public interface IExplorationSubmitListener : IExplorationEventListener
    {
        void OnExplorationRequest(EnumExploration.ExplorationType type);

    }

    public interface IExplorationOnCombatListener : IExplorationEventListener
    {
        /// <summary>
        /// Event call from Exploration Element (combat type).
        /// </summary>
        /// <param name="type">If the combat is a basic, elite or boss combat</param>
        void OnExplorationCombatLoadFinish(EnumExploration.ExplorationType type);

        void OnExplorationReturnFromCombat(EnumExploration.ExplorationType fromCombatType);
    }
}
