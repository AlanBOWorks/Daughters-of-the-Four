using System;
using CombatSystem.Animator;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatEntity
{
    public class UEntityHolder : MonoBehaviour
    {
        [Title("References")]
        [SerializeField]
        private Transform uiPivot;

        [Title("Parameters")]
        [SerializeField] private Vector3 spawnLocalPosition;
        [Title("Events")]
        [SerializeField] private UEntityHolderListener[] listeners = new UEntityHolderListener[0];

        [Title("Behaviour")] 
        [NonSerialized]
        public ICombatAnimationHandler AnimationHandler;


        public Transform GetUIPivot() => uiPivot;

        private void Awake()
        {
            foreach (var listener in listeners)
            {
                listener.Inject(this);
            }
        }

        public void Inject(CombatingEntity entity)
        {
            foreach (var listener in listeners)
            {
                listener.Inject(entity);
            }
        }

        public void HandleTransformSpawn(Transform onRespawn)
        {
            var transform1 = transform;
            transform1.parent = onRespawn;
            transform1.localPosition = spawnLocalPosition;
            transform1.localRotation = Quaternion.identity;
        }

    }

    public abstract class UEntityHolderListener : MonoBehaviour, IEntityHolderListener
    {
        protected CombatingEntity CurrentEntity;

        public void Inject(CombatingEntity entity) => CurrentEntity = entity;
        public abstract void Inject(UEntityHolder holder);
    }

    public interface IEntityHolderListener
    {
        void Inject(CombatingEntity entity);
        void Inject(UEntityHolder holder);
    }
}
