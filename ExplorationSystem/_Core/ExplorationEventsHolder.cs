using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ExplorationSystem
{
    public sealed class ExplorationEventsHolder : IExplorationEventsHolder
    {
        public ExplorationEventsHolder()
        {
            _worldSceneListeners = new HashSet<IWorldSceneChangeListener>();
            _explorationSubmitListeners = new HashSet<IExplorationSubmitListener>();
            _onCombatListeners = new HashSet<IExplorationOnCombatListener>();
        }

        [ShowInInspector,HorizontalGroup("Scene Listeners")] 
        private readonly HashSet<IWorldSceneChangeListener> _worldSceneListeners;

        [ShowInInspector,HorizontalGroup("Element Listeners")] 
        private readonly HashSet<IExplorationSubmitListener> _explorationSubmitListeners;
        [ShowInInspector,HorizontalGroup("Element Listeners")] 
        private readonly HashSet<IExplorationOnCombatListener> _onCombatListeners;
       

        public void Subscribe(IExplorationEventListener listener)
        {
            if (listener is IWorldSceneChangeListener worldSceneListener)
                _worldSceneListeners.Add(worldSceneListener);

            if (listener is IExplorationSubmitListener submitListener)
                _explorationSubmitListeners.Add(submitListener);
            if (listener is IExplorationOnCombatListener combatListener)
                _onCombatListeners.Add(combatListener);
        }
        public void UnSubscribe(IExplorationEventListener listener)
        {
            if (listener is IWorldSceneChangeListener worldSceneListener)
                _worldSceneListeners.Remove(worldSceneListener);

            if (listener is IExplorationSubmitListener submitListener)
                _explorationSubmitListeners.Remove(submitListener);
            if (listener is IExplorationOnCombatListener combatListener)
                _onCombatListeners.Remove(combatListener);
        }

        public void OnWorldSceneEnters(IExplorationSceneDataHolder lastMap)
        {
            foreach (var listener in _worldSceneListeners) 
                listener.OnWorldSceneEnters(lastMap);
        }

        public void OnWorldSceneSubmit(IExplorationSceneDataHolder targetMap)
        {
            foreach (var listener in _worldSceneListeners) 
                listener.OnWorldSceneSubmit(targetMap);
        }

        public void OnWorldSelectSceneLoad(IExplorationSceneDataHolder loadedMap)
        {
            foreach (var listener in _worldSceneListeners)
                listener.OnWorldSelectSceneLoad(loadedMap);
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


    internal interface IExplorationEventsHolder : 
        IWorldSceneChangeListener,
        IExplorationSubmitListener,
        IExplorationOnCombatListener
    { }    
    public interface IExplorationEventListener { }


    public interface IWorldSceneChangeListener : IExplorationEventListener
    {
        /// <summary>
        /// Event call once the player goes the World Selection Scene by Menu or returns after defeating the Bosses;
        /// </summary>
        /// <param name="lastMap">The map which the player came from after defeating the Boss of the level;<br></br>
        /// >Note: In case of [NULL] it means that the previous map was the CharacterSelector or loading screen</param>
        void OnWorldSceneEnters(IExplorationSceneDataHolder lastMap);
        /// <summary>
        /// Event call when there's a change from the World Selection Scene(as main) to another scene(generally
        /// towards an Exploration Scene)
        /// </summary>
        /// <param name="targetMap">The target map towards the player is switching towards to;<br></br>
        /// >Note: in case of NULL it means is leaving towards the Main Menu from the World Map selection Scene</param>
        void OnWorldSceneSubmit(IExplorationSceneDataHolder targetMap);

        /// <summary>
        /// Event call on the very first frame after loading the map.
        /// </summary>
        void OnWorldSelectSceneLoad(IExplorationSceneDataHolder loadedMap);
    }
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
