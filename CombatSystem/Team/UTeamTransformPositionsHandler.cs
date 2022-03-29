using System;
using CombatSystem._Core;
using CombatSystem.Entity;
using UnityEngine;

namespace CombatSystem.Team
{
    public class UTeamTransformPositionsHandler : MonoBehaviour, 
        IOppositionTeamStructureRead<ITeamPositionHandler>
    {
        [SerializeField] 
        private TeamPositionHandler playerTeamPositionHandler = new TeamPositionHandler();
        [SerializeField] 
        private TeamPositionHandler enemyTeamPositionHandler = new TeamPositionHandler();
        
        public ITeamPositionHandler PlayerTeamType => playerTeamPositionHandler;
        public ITeamPositionHandler EnemyTeamType => enemyTeamPositionHandler;



        private void Awake()
        {
            HandleSingleton();
        }

        private void HandleSingleton()
        {
            var spawnHandler = CombatSystemSingleton.PositionHandler;

            var singletonReference = spawnHandler.TransformPositionsHandlerReference;
            if (singletonReference != null) 
                Destroy(singletonReference);

            spawnHandler.TransformPositionsHandlerReference = this;
        }
        

        [Serializable]
        private sealed class TeamPositionHandler : ITeamFullPositionStructureRead<Transform>, ITeamPositionHandler
        {
            [SerializeField]
            private Transform frontLineType;
            [SerializeField]
            private Transform midLineType;
            [SerializeField]
            private Transform backLineType;
            [SerializeField]
            private Transform flexLineType;


            public Transform FrontLineType => frontLineType;
            public Transform MidLineType => midLineType;
            public Transform BackLineType => backLineType;

            public Transform FlexLineType => flexLineType;


            public void HandleTeamMembersPosition(in CombatTeam team)
            {
                foreach (CombatEntity member in team)
                {
                    HandleMember(in member);
                }
            }

            private void HandleMember(in CombatEntity member)
            {
                var memberPositioning = member.PositioningType;
                Transform targetReferencePoint = UtilsTeam.GetElement(memberPositioning, this);
                HandleMemberPosition(in member, in targetReferencePoint);
            }

            private void HandleMemberPosition(in CombatEntity member, in Transform referencePoint)
            {
                Transform memberTransform = member.InstantiationReference.transform;

                memberTransform.position = referencePoint.position;
                memberTransform.rotation = referencePoint.rotation;
            }

            public void ProvideInstantiationPoint(in ICombatEntityProvider provider, out Vector3 position, out Quaternion rotation)
            {
                var positioning = provider.GetAreaData().PositioningType;
                Transform targetReferencePoint = UtilsTeam.GetElement(positioning, this);
                position = targetReferencePoint.position;
                rotation = targetReferencePoint.rotation;
            }

        }
    }
}
