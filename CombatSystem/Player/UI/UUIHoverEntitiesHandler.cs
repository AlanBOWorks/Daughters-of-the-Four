using System;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UUIHoverEntitiesHandler : UTeamElementSpawner<UUIHoverEntityHolder>
    {
        [Title("Anchor Values")]
        [SerializeField] private Canvas canvasHolder;
        




        private void Start()
        {
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
        }
        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }

        private const float CentralPointCalculationConstant = .5f;
        private const float CanvasTeamLengthModifier = .25f;
        private float _centralPointHorizontalPoint;
        private float _canvasHorizontalLength;
        private float _verticalCentralPoint;
        private void OnRectTransformDimensionsChange()
        {
            //todo Change the canvas for an PositionPivotPoint from WorldToScreenPointCalculation

            var canvasRect = canvasHolder.pixelRect;

            _centralPointHorizontalPoint = CentralPointCalculationConstant * canvasRect.width;
            _canvasHorizontalLength = _centralPointHorizontalPoint * CanvasTeamLengthModifier;

            _verticalCentralPoint = CentralPointCalculationConstant * canvasRect.height;
            RecalculatePoints();
            _canRecalculate = true;
        }

        private bool _canRecalculate;
        private void RecalculatePoints()
        {
            if(!_canRecalculate) return;

            foreach ((CombatEntity entity, UUIHoverEntityHolder element) in GetDictionary())
            {
                bool isPlayerElement = UtilsTeam.IsPlayerTeam(entity);
                InjectAnchorPoint(entity,element,isPlayerElement);
            }
        }

        private const float HeightDistance = 80;
        protected override void OnCreateElement(in CreationValues creationValues)
        {
            var element = creationValues.Element;
            var entity = creationValues.Entity;
            var isPlayerElement = creationValues.IsPlayerElement;

            base.OnCreateElement(in creationValues);
            element.Show();
            element.EntityInjection(entity);
            InjectAnchorPoint(entity,element,isPlayerElement);
        }

        private void InjectAnchorPoint(CombatEntity entity, UUIHoverEntityHolder element, bool isPlayerElement)
        {

            float roleIndex = UtilsTeam.GetRoleIndex(entity);
            float horizontalPoint = roleIndex * _canvasHorizontalLength;
            if (!isPlayerElement)
                horizontalPoint += _centralPointHorizontalPoint;
            float verticalPoint = CalculateHeightPoint(entity);

            element.InjectAnchorPosition(new Vector2(horizontalPoint, verticalPoint));
        }

        private float CalculateHeightPoint(CombatEntity entity)
        {
            float heightPointModifier;
            switch (entity.RolePriorityType)
            {
                case EnumTeam.RolePriorityType.SecondaryRole:
                    heightPointModifier = 1;
                    break;
                case EnumTeam.RolePriorityType.ThirdRole:
                    heightPointModifier = 2;
                    break;
                default:
                    heightPointModifier = 0;
                    break;
            }

            return heightPointModifier * HeightDistance + _verticalCentralPoint;
        }

    }
}
