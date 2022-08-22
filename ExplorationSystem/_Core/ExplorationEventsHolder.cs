using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace ExplorationSystem
{
    public sealed class ExplorationEventsHolder : IExplorationEventsHolder
    {
        public ExplorationEventsHolder()
        {
            _worldSceneListeners = new HashSet<IWorldSceneListener>();
            _sceneChangeListeners = new HashSet<ISceneChangeListener>();
        }

        [ShowInInspector] 
        private readonly HashSet<IWorldSceneListener> _worldSceneListeners;
        [ShowInInspector]
        private readonly HashSet<ISceneChangeListener> _sceneChangeListeners;

       

        public void Subscribe(IExplorationEventListener listener)
        {
            if (listener is ISceneChangeListener sceneChangeListener)
                _sceneChangeListeners.Add(sceneChangeListener);
            if (listener is IWorldSceneListener worldSceneListener)
                _worldSceneListeners.Add(worldSceneListener);
        }
        public void UnSubscribe(IExplorationEventListener listener)
        {
            if (listener is ISceneChangeListener sceneChangeListener)
                _sceneChangeListeners.Remove(sceneChangeListener);
            if (listener is IWorldSceneListener worldSceneListener)
                _worldSceneListeners.Remove(worldSceneListener);
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
    }


    internal interface IExplorationEventsHolder : ISceneChangeListener, IWorldSceneListener{}    
    public interface IExplorationEventListener { }
}
