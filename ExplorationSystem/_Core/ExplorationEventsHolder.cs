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
        }

        [ShowInInspector] 
        private readonly HashSet<IWorldSceneListener> _worldSceneListeners;
        [ShowInInspector]
        private readonly HashSet<ISceneChangeListener> _sceneChangeListeners;

        [ShowInInspector] 
        private readonly HashSet<IExplorationSubmitListener> _explorationSubmitListeners;
       

        public void Subscribe(IExplorationEventListener listener)
        {
            if (listener is ISceneChangeListener sceneChangeListener)
                _sceneChangeListeners.Add(sceneChangeListener);
            if (listener is IWorldSceneListener worldSceneListener)
                _worldSceneListeners.Add(worldSceneListener);
            if (listener is IExplorationSubmitListener submitListener)
                _explorationSubmitListeners.Add(submitListener);
        }
        public void UnSubscribe(IExplorationEventListener listener)
        {
            if (listener is ISceneChangeListener sceneChangeListener)
                _sceneChangeListeners.Remove(sceneChangeListener);
            if (listener is IWorldSceneListener worldSceneListener)
                _worldSceneListeners.Remove(worldSceneListener);
            if (listener is IExplorationSubmitListener submitListener)
                _explorationSubmitListeners.Remove(submitListener);
        }

        public void OnSceneChange(IExplorationSceneDataHolder sceneData)
        {
            foreach (var listener in _sceneChangeListeners) 
                listener.OnSceneChange(sceneData);
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

            UtilsExplorationMechanics.InvokeExplorationBehaviourType(type);
        }
    }


    internal interface IExplorationEventsHolder : ISceneChangeListener, IWorldSceneListener,
        IExplorationSubmitListener
    {}    
    public interface IExplorationEventListener { }


    public interface IExplorationSubmitListener : IExplorationEventListener
    {
        void OnExplorationRequest(EnumExploration.ExplorationType type);

    }
}
