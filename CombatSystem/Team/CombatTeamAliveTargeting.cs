using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using UnityEngine;

namespace CombatSystem.Team
{
    public sealed class CombatTeamAliveTargeting : FullPositionMainGroupClass<List<CombatEntity>>
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
            AddOnlyFirstAlive(mainGroup);


            return _handlingMembers;
        }

        public IReadOnlyList<CombatEntity> GetAliveLineMembers(EnumTeam.Positioning targetPositioning)
        {
            _handlingMembers.Clear();

            switch (targetPositioning)
            {
                case EnumTeam.Positioning.FrontLine:
                    AddOnlyFirstAlive(FrontLineType);
                    break;
                case EnumTeam.Positioning.MidLine:
                    AddOnlyFirstAlive(MidLineType);
                    AddOnlyFirstAlive(FlexLineType); //Flex is in Mid Position
                    break;
                case EnumTeam.Positioning.BackLine:
                    AddOnlyFirstAlive(BackLineType);
                    break;
                case EnumTeam.Positioning.FlexLine:
                    AddOnlyFirstAlive(FlexLineType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetPositioning), targetPositioning, null);
            }
            return _handlingMembers;
        }


        public IReadOnlyList<CombatEntity> GetFrontMostAliveLineMembers()
        {
            _handlingMembers.Clear();
            AddOnlyFirstAlive(FrontLineType);

            if(_handlingMembers.Count <= 0)
            {
                AddOnlyFirstAlive(MidLineType);
                AddOnlyFirstAlive(FlexLineType); //Flex is in Mid Position
            }

            if(_handlingMembers.Count <= 0)
                AddOnlyFirstAlive(BackLineType);


            return _handlingMembers;
        }

        public IReadOnlyList<CombatEntity> GetAliveMainPositionMembers()
        {
            _handlingMembers.Clear();

            AddBreakOnlyFirstAlive(FrontLineType);
            AddBreakOnlyFirstAlive(MidLineType);
            AddBreakOnlyFirstAlive(BackLineType);
            return _handlingMembers;
        }

        public IReadOnlyList<CombatEntity> GetAllAliveMembers()
        {
            _handlingMembers.Clear();
            AddOnlyFirstAlive(_team);

            return _handlingMembers;
        }

        public IReadOnlyList<CombatEntity> GetAllAliveMembers(in CombatEntity excludeMember)
        {
            _handlingMembers.Clear();
            AddOnlyFirstAlive(_team);
            if (_handlingMembers.Contains(excludeMember)) _handlingMembers.Remove(excludeMember);

            return _handlingMembers;
        }

        private void AddBreakOnlyFirstAlive(IEnumerable<CombatEntity> members)
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
        private void AddOnlyFirstAlive(IEnumerable<CombatEntity> members)
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
