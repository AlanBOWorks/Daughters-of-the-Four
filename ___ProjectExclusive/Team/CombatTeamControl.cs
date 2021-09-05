using System;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace _Team
{
    public class CombatTeamControl : IStanceProvider
    {
        public CombatTeamControl()
        {
            _normalStance = EnumTeam.Stances.Neutral;
        }


        [ShowInInspector]
        [Range(-1,1)]
        private float _teamControlAmount = 0;


        [ShowInInspector] 
        public float BurstControlAmount { get; private set; }


        public float GetControlAmount()
        {
            float control = _teamControlAmount + BurstControlAmount;

            return control;
        }

        public void VariateControl(float amount)
        {
            _teamControlAmount += amount;
            _teamControlAmount = Mathf.Clamp(_teamControlAmount, -1, 1);
        }

        public void MirrorEnemy(CombatTeamControl enemyControl)
        {
            _teamControlAmount = -enemyControl._teamControlAmount;
        }

        public const float DefaultLosingThreshold = -.5f;
        public bool IsLosing()
        {
            return _teamControlAmount < DefaultLosingThreshold;
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
