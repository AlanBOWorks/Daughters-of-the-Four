using System.Collections.Generic;
using CombatEntity;
using CombatTeam;
using UnityEngine;

namespace __ProjectExclusive.Player
{
    public class UCanvasPivotOverEntities : UPersistentTeamStructurePoolerBase<UPivotOverEntity>
    {
        private void Start()
        {
            _listeners = new HashSet<ICanvasPivotOverEntityListener>();
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

    internal interface ICanvasPivotOverEntityListener
    {
        void OnPooledElement(CombatingEntity user, UPivotOverEntity pivotOverEntity);
    }
}
