using CombatEntity;
using UnityEngine;

namespace Stats
{
    public class FollowUpStats : IBaseStatsRead<float>
    {
        private IOffensiveStatsRead<float> _followerOffensiveStats;

        public void SwitchFollower(CombatingEntity user)
        {
            _followerOffensiveStats = user.CombatStats;
        }
       
        public float Attack => _followerOffensiveStats?.Attack ?? 0;
        public float Persistent => _followerOffensiveStats?.Persistent ?? 0;
        public float Debuff => _followerOffensiveStats?.Debuff ?? 0;
        public float FollowUp => _followerOffensiveStats?.FollowUp ?? 0;
        public float Heal => 0;
        public float Buff => 0;
        public float ReceiveBuff => 0;
        public float Shielding => 0;
        public float MaxHealth => 0;
        public float MaxMortality => 0;
        public float DebuffResistance => 0;
        public float DamageResistance => 0;
        public float InitiativeSpeed => 0;
        public float InitialInitiative => 0;
        public float ActionsPerSequence => 0;
        public float Critical => 0;
    }

    public class FollowUpHandler
    {
        public FollowUpHandler(CombatingEntity user)
        {
            _user = user;
        }

        private readonly CombatingEntity _user;
        /// <summary>
        /// If you're a support, you're gonna help the attacker
        /// </summary>
        private CombatingEntity _currentFollowAttacker; // This is to keep track the attacker to remove its follower (this)


        public void SwitchFollowTarget(CombatingEntity followAttacker)
        {
            if (_currentFollowAttacker != null)
            {
                SwitchFollow(_currentFollowAttacker,null);
            }
            _currentFollowAttacker = followAttacker;
            SwitchFollow(followAttacker, _user);
        }

        private static void SwitchFollow(CombatingEntity holder, CombatingEntity follower)
        {
            var followUp = GetFollowStats(holder);
            followUp.SwitchFollower(follower);
        }

        private static FollowUpStats GetFollowStats(CombatingEntity entity)
        {
            return entity.CombatStats.GetFollowUpStat();
        }


    }
}
