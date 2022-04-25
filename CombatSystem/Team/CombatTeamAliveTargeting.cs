using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using UnityEngine;

namespace CombatSystem.Team
{
    public sealed class CombatTeamAliveTargeting : FlexPositionMainGroupClass<List<CombatEntity>>
    {
        public CombatTeamAliveTargeting(CombatTeam team) : base()
        {
            _team = team;
            _handlingMembers = new List<CombatEntity>();
        }

        private readonly CombatTeam _team;
        private readonly List<CombatEntity> _handlingMembers;

        public void AddMember(in CombatEntity member)
        {
            var positioning = member.PositioningType;
            var collection = UtilsTeam.GetElement(positioning, this);
            if(collection.Contains(member)) return;

            collection.Add(member);
        }

        public void RemoveMember(in CombatEntity member)
        {
            var positioning = member.PositioningType;
            var collection = UtilsTeam.GetElement(positioning, this);
            if (collection.Contains(member))
                collection.Remove(member);
        }


        public IReadOnlyList<CombatEntity> GetAlivePositionMembers(EnumTeam.Positioning targetPositioning)
        {
            _handlingMembers.Clear();

            var mainGroup = UtilsTeam.GetElement(targetPositioning, this);
            AddOnlyAlive(mainGroup);


            return _handlingMembers;
        }

        public IReadOnlyList<CombatEntity> GetAliveLineMembers(EnumTeam.Positioning targetPositioning)
        {
            _handlingMembers.Clear();

            switch (targetPositioning)
            {
                case EnumTeam.Positioning.FrontLine:
                    AddOnlyAlive(FrontLineType);
                    break;
                case EnumTeam.Positioning.MidLine:
                    AddOnlyAlive(MidLineType);
                    AddOnlyAlive(FlexLineType); //Flex is in Mid Position
                    break;
                case EnumTeam.Positioning.BackLine:
                    AddOnlyAlive(BackLineType);
                    break;
                case EnumTeam.Positioning.FlexLine:
                    AddOnlyAlive(FlexLineType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetPositioning), targetPositioning, null);
            }
            return _handlingMembers;
        }


        public IReadOnlyList<CombatEntity> GetFrontMostAliveLineMembers()
        {
            _handlingMembers.Clear();
            AddOnlyAlive(FrontLineType);

            if(_handlingMembers.Count <= 0)
            {
                AddOnlyAlive(MidLineType);
                AddOnlyAlive(FlexLineType); //Flex is in Mid Position
            }

            if(_handlingMembers.Count <= 0)
                AddOnlyAlive(BackLineType);


            return _handlingMembers;
        }

        public IReadOnlyList<CombatEntity> GetAliveMainPositionMembers()
        {
            _handlingMembers.Clear();

            AddOnlyFirstAlive(FrontLineType);
            AddOnlyFirstAlive(MidLineType);
            AddOnlyFirstAlive(BackLineType);
            return _handlingMembers;
        }

        public IReadOnlyList<CombatEntity> GetAllAliveMembers()
        {
            _handlingMembers.Clear();
            AddOnlyAlive(_team);

            return _handlingMembers;
        }

        public IReadOnlyList<CombatEntity> GetAllAliveMembers(in CombatEntity excludeMember)
        {
            _handlingMembers.Clear();
            AddOnlyAlive(_team);
            if (_handlingMembers.Contains(excludeMember)) _handlingMembers.Remove(excludeMember);

            return _handlingMembers;
        }

        private void AddOnlyFirstAlive(IEnumerable<CombatEntity> members)
        {
            foreach (var member in members)
            {
                if (member.CanBeTarget())
                {
                    _handlingMembers.Add(member);
                    break;
                }
            }
        }
        private void AddOnlyAlive(IEnumerable<CombatEntity> members)
        {
            foreach (var member in members)
            {
                if (member.CanBeTarget())
                {
                    _handlingMembers.Add(member);
                }
            }
        }

    }

}
