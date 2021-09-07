using System;
using System.Collections.Generic;
using _CombatSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Characters
{
    /// <summary>
    /// Offensive [<seealso cref="Skills.CombatSkill"/>] will be re-directed (if percentage/randomness applies).
    /// <br></br><br></br>
    ///
    /// Note: <br></br>
    /// - Guard/Guarding: means that this will receive the effect instead of the [Target] <br></br>
    /// - Protect/ProtectMe: means that Me/user will not receive the effect but instead a Guardian
    /// </summary>
    public class CharacterGuarding : ITempoListenerVoid, ICharacterEventListener
    {
        public CharacterGuarding(CombatingEntity user)
        {
            _user = user;
            ProtectedBy = null;

            _guardingEntities = new List<CombatingEntity>();
        }

        private readonly CombatingEntity _user;

        [ShowInInspector]
        public CombatingEntity ProtectedBy { get; private set; }
        [NonSerialized,ShowInInspector,PropertyRange(-3,3)]
        public float ProtectionPercentage;

        [ShowInInspector]
        private readonly List<CombatingEntity> _guardingEntities;

        public bool HasProtector() => ProtectedBy != null;

        public void GuardTarget(CombatingEntity target, float protectionPercentage)
        {
            target.Guarding.ProtectMe(_user,protectionPercentage);
            _guardingEntities.Add(target);
        }

        public void ProtectMe(CombatingEntity guarder, float protectionPercentage)
        {
            if (HasProtector())
            {
                ProtectedBy.Guarding.RemoveGuarding(_user);
            }

            ProtectedBy = guarder;
            ProtectionPercentage = protectionPercentage;
        }

        /// <summary>
        /// Removes the [<seealso cref="CombatingEntity"/>] that is Protecting Me
        /// </summary>
        public void RemoveProtection()
        {
            if (ProtectedBy == null) return;

            ProtectedBy = null;
            ProtectionPercentage = 0;
        }

        /// <summary>
        /// Removes all [<seealso cref="CombatingEntity"/>]s that this is Guarding
        /// </summary>
        public void RemoveGuarding()
        {
            for (int i = _guardingEntities.Count - 1; i >= 0; i--)
            {
                _guardingEntities[i].Guarding.RemoveProtection();
                _guardingEntities.RemoveAt(i);
            }
        }
        /// <summary>
        /// Removes the [<seealso cref="CombatingEntity"/>] that is being protected by this
        /// </summary>
        public void RemoveGuarding(CombatingEntity protectedEntity)
        {
            _guardingEntities.Remove(protectedEntity);
        }


        public void VariateTarget(ref CombatingEntity possibleTarget)
        {
            Debug.Log($"Guarding: {possibleTarget.CharacterName}");
            if (!HasProtector() || Random.value > ProtectionPercentage) return;

            possibleTarget = ProtectedBy;
            Debug.Log($"Guarded by: {possibleTarget.CharacterName}");
        }

        public void OnInitiativeTrigger()
        {
            RemoveGuarding();
        }

        public void OnDoMoreActions()
        {}

        public void OnFinisAllActions()
        {} 
    }
}
