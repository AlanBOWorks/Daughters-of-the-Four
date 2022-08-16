using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ExplorationSystem
{
    public sealed class ExplorationEventsHolder : ISceneChangeListener
    {
        public ExplorationEventsHolder()
        {
            _sceneChangeListeners = new HashSet<ISceneChangeListener>();
        }

        [ShowInInspector]
        private readonly HashSet<ISceneChangeListener> _sceneChangeListeners;

        public void Subscribe(IExplorationEventListener listener)
        {
            if (listener is ISceneChangeListener sceneChangeListener)
                _sceneChangeListeners.Add(sceneChangeListener);
        }
        public void UnSubscribe(IExplorationEventListener listener)
        {
            if (listener is ISceneChangeListener sceneChangeListener)
                _sceneChangeListeners.Remove(sceneChangeListener);
        }

        public void OnSceneChange(IExplorationSceneDataHolder sceneData)
        {
            foreach (var listener in _sceneChangeListeners)
            {
                listener.OnSceneChange(sceneData);
            }
        }
    }



    public interface IExplorationEventListener { }
}
