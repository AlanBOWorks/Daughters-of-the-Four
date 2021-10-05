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
            transform.parent = onRespawn;
            transform.localPosition = spawnLocalPosition;
        }

        private CombatingEntity _entity;
    }
}
