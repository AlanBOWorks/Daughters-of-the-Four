using CombatSystem.Team;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CombatSystem.Entity
{
    /// <summary>
    /// Spawns the entities in the scene in a predefined position; For manual position each scene should afterwards
    /// be invoked in their specific [<seealso cref="MonoBehaviour"/>]
    /// </summary>
    public sealed class SpawnEntityPositionHandler : IOppositionTeamStructureRead<ITeamPositionHandler>
    {

        public UTeamPositionsHandler PositionsHandlerReference;
        private readonly ITeamPositionHandler _playerTeamType;
        private readonly ITeamPositionHandler _enemyTeamType;

        public SpawnEntityPositionHandler()
        {
            _playerTeamType = new BackUpPositionHandler(-1);
            _enemyTeamType = new BackUpPositionHandler(1);
        }


        public ITeamPositionHandler PlayerTeamType => PositionsHandlerReference != null 
            ? PositionsHandlerReference.PlayerTeamType 
            : _playerTeamType;

        public ITeamPositionHandler EnemyTeamType => PositionsHandlerReference != null
            ? PositionsHandlerReference.EnemyTeamType
            : _enemyTeamType;

        private sealed class BackUpPositionHandler : ITeamPositionHandler
            {
                public BackUpPositionHandler(float distanceModifier)
                {
                    _distanceModifier = distanceModifier;
                    _rotation = Quaternion.LookRotation(Vector3.right * distanceModifier);
                }

                private const float LateralDistance = 2f;
                private readonly float _distanceModifier;
                private readonly Quaternion _rotation;
                public void ProvideInstantiationPoint(in ICombatEntityProvider provider, 
                    out Vector3 position,
                    out Quaternion rotation)
                {
                    var positioning = provider.GetAreaData().PositioningType;
                    float xPoint =1; 
                    xPoint+= (float) positioning;
                    xPoint *= _distanceModifier * LateralDistance;

                    Vector2 randomVariation = Random.insideUnitCircle;
                    position = new Vector3(xPoint + randomVariation.x *.1f,0,randomVariation.y);
                    rotation = _rotation;
                }
            }

    }

    public interface ITeamPositionHandler
    {
        void ProvideInstantiationPoint(in ICombatEntityProvider provider, out Vector3 position, out Quaternion rotation);
    }
}
