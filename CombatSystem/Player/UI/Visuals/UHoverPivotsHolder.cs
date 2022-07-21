using System;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UHoverPivotsHolder : MonoBehaviour
    {
        [SerializeField] private Canvas canvasHolder;
        [SerializeField, SuffixLabel("px")] private float offRoleVerticalOffset = 120f;

        private PivotHolder _playerMainPivotsHolder;
        [ShowInInspector,DisableInEditorMode]
        private TeamPivotsHolder _enemyPivotsHolder;


        private const float InitialHorizontalOffset = 0;
        private void Awake()
        {
            _playerMainPivotsHolder 
                = new PivotHolder(offRoleVerticalOffset);
            _enemyPivotsHolder 
                = new TeamPivotsHolder(offRoleVerticalOffset);
            UpdatePoints();
        }

        private void OnRectTransformDimensionsChange()
        {
           UpdatePoints();
        }

        private void UpdatePoints()
        {
            if(_playerMainPivotsHolder == null) return;

            float canvasWidth = canvasHolder.pixelRect.width;
            _playerMainPivotsHolder.HandlePoints(canvasWidth, InitialHorizontalOffset, true);
            _enemyPivotsHolder.HandlePoints(canvasWidth, InitialHorizontalOffset, false);
        }

        private sealed class PivotHolder : ITeamFlexStructureRead<Vector3>
        {
            [ShowInInspector]
            public Vector3 VanguardType { get; private set; }
            [ShowInInspector]
            public Vector3 AttackerType { get; private set; }
            [ShowInInspector]
            public Vector3 SupportType { get; private set; }
            [ShowInInspector]
            public Vector3 FlexType { get; private set; }


            public PivotHolder(float spawnHeight)
            {
                SpawnHeight = spawnHeight;
            }

            public PivotHolder(float spawnHeight, float canvasWidth, float initialOffset, bool invertSpacing) :
                this(spawnHeight)
            {
                HandlePoints(canvasWidth,initialOffset,invertSpacing);
            }

            public float SpawnHeight;


            private const float CanvasModifier = .25f * .5f; // Four elements by the half of the screen
            public void HandlePoints(float canvasWidth, float initialOffset, bool invertSpacing)
            {
                float elementsSpacing = canvasWidth * .25f;

                if (invertSpacing)
                {
                    elementsSpacing *= -1;
                    initialOffset *= -1;
                }

                VanguardType = CalculatePoint(0);
                AttackerType = CalculatePoint(1);
                SupportType = CalculatePoint(2);
                FlexType = CalculatePoint(3);


                Vector3 CalculatePoint(float modifier)
                {
                    float horizontalPoint = initialOffset + elementsSpacing * modifier;

                    return new Vector3(horizontalPoint, SpawnHeight, 0);
                }
            }

            [Button]
            private void TestObjects(GameObject spawnStuff)
            {
                var enumerable = UtilsTeam.GetEnumerable(this);
                var onParent = spawnStuff.transform.parent;
                foreach (var point in enumerable)
                {
                    var thing = Instantiate(spawnStuff, onParent);
                    thing.transform.position = point;
                }
            }
        }

        private sealed class TeamPivotsHolder : ITeamAlimentStructureRead<ITeamFlexStructureRead<Vector3>>
        {
            [ShowInInspector]
            private readonly PivotHolder _mainPivots;
            [ShowInInspector]
            private readonly PivotHolder _secondaryPivots;
            [ShowInInspector]
            private readonly PivotHolder _thirdPivots;

            public TeamPivotsHolder(float spawnHeight)
            {
                _mainPivots = new PivotHolder(0);
                _secondaryPivots = new PivotHolder(spawnHeight);
                _thirdPivots = new PivotHolder(2 * spawnHeight);
            }

            public TeamPivotsHolder(float spawnHeight, float canvasWidth, float initialOffset, bool toTheLeft) 
            : this(spawnHeight)
            {
                HandlePoints(canvasWidth,initialOffset, toTheLeft);
            }

            public void HandlePoints(float canvasWidth, float initialOffset, bool invertSpacing)
            {
                _mainPivots.HandlePoints(canvasWidth,initialOffset,invertSpacing);
                _secondaryPivots.HandlePoints(canvasWidth,initialOffset,invertSpacing);
                _thirdPivots.HandlePoints(canvasWidth,initialOffset,invertSpacing);
            }

            public ITeamFlexStructureRead<Vector3> MainRole => _mainPivots;
            public ITeamFlexStructureRead<Vector3> SecondaryRole => _secondaryPivots;
            public ITeamFlexStructureRead<Vector3> ThirdRole => _thirdPivots;
        }
    }
}
