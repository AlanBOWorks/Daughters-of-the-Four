using System;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace _Team
{
    public class TeamCombatControlHandler : IStanceProvider
    {
        public TeamCombatControlHandler()
        {
            LoseControlThreshold = DefaultLosingThreshold;
            _normalStance = EnumTeam.Stances.Neutral;
        }


        [field: ShowInInspector]
        [field: Range(-1,1)]
        public float TeamControlAmount = 0;


        [ShowInInspector] 
        public float BurstControlAmount { get; private set; }
        public readonly float LoseControlThreshold;

        public const float DefaultLosingThreshold = -.5f;

        public float GetControlAmount()
        {
            float control = TeamControlAmount + BurstControlAmount;

            return control;
        }

        public bool IsLosing()
        {
            return TeamControlAmount < LoseControlThreshold;
        }

        public EnumTeam.Stances CurrentStance => IsForcedStance ? ForceStance : _normalStance;
        [ShowInInspector]
        public EnumTeam.Stances ForceStance { get; private set; }

        [ShowInInspector]
        private EnumTeam.Stances _normalStance;

        [ShowInInspector] 
        public bool IsForcedStance;

        [ShowInInspector] 
        public bool IsBurstStance;


        public void VariateStance(EnumTeam.Stances target)
        {
            _normalStance = target;
        }

        public void DoForceStance(EnumTeam.Stances target)
        {
            IsForcedStance = true;
            ForceStance = target;
        }

        public void FinishForceStance()
        {
            IsForcedStance = false;
        }

        public void DoBurstControl(float targetBurst)
        {
            BurstControlAmount = targetBurst;
            IsBurstStance = true;
        }

        public void DoBurstVariation(float variation)
        {
            BurstControlAmount += variation;
        }

        public void FinishBurstControl()
        {
            BurstControlAmount = 0;
            IsBurstStance = false;
        }
    }

}
