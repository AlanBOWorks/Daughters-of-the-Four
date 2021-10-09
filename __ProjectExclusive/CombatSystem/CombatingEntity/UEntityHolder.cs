using UnityEngine;

namespace CombatEntity
{
    public class UEntityHolder : MonoBehaviour
    {
        [SerializeField] private Vector3 spawnLocalPosition;

        public void Inject(CombatingEntity entity)
        {
            _entity = entity;
        }

        public void HandleTransformSpawn(Transform onRespawn)
        {
            var transform1 = transform;
            transform1.parent = onRespawn;
            transform1.localPosition = spawnLocalPosition;
        }

        private CombatingEntity _entity;
    }
}
