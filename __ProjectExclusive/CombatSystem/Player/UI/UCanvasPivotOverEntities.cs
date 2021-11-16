using System;
using System.Collections.Generic;
using CombatEntity;
using CombatTeam;
using Sirenix.OdinInspector;
using UnityEngine;

namespace __ProjectExclusive.Player.UI
{
    public class UCanvasPivotOverEntities : UPersistentTeamStructurePoolerBase<UPivotOverEntity>
    {
        private void Start()
        {
            _listeners = new HashSet<ICanvasPivotOverEntityListener>();
            var collectedComponents = GetComponents<ICanvasPivotOverEntityListener>();
            foreach (var listener in collectedComponents)
            {
                _listeners.Add(listener);
            }
        }

        private HashSet<ICanvasPivotOverEntityListener> _listeners;
        internal ICollection<ICanvasPivotOverEntityListener> PoolListeners => _listeners;

        protected override void OnPoolElement(ref UPivotOverEntity instantiatedElement)
        {
            
        }

        protected override void OnPreparationEntity(CombatingEntity entity, UPivotOverEntity element)
        {
            element.Injection(entity);
            foreach (ICanvasPivotOverEntityListener listener in _listeners)
            {
                listener.OnPooledElement(entity,element);
            }
        }

        public override void OnAfterLoadsCombat()
        {
        }

        public override void OnCombatPause()
        {
            throw new System.NotImplementedException();
        }

        public override void OnCombatResume()
        {
            throw new System.NotImplementedException();
        }
    }

    public abstract class UCanvasPivotOverEntityListener : MonoBehaviour, ICanvasPivotOverEntityListener
    {
#if UNITY_EDITOR
        [HideInPlayMode, HideIf("hideThisBox"),
         InfoBox("This needs to be in the same level as [UCanvasPivotOverEntities] for the GetComponents<>")]
        public bool hideThisBox = false;
#endif
        public abstract void OnPooledElement(CombatingEntity user, UPivotOverEntity pivotOverEntity);
    }

    internal interface ICanvasPivotOverEntityListener
    {
        void OnPooledElement(CombatingEntity user, UPivotOverEntity pivotOverEntity);
    }
}
