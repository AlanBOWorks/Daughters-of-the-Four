using System;
using System.Collections.Generic;
using Animancer;
using CombatSkills;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CombatSystem.Animator
{
    public class UCombatAnimatorAnimancer : UCombatAnimationsHandler
    {
        [Title("References")]
        [SerializeField] private AnimancerComponent animancer;
        [SerializeField,TabGroup("Performing Animations")]
        private AnimationsHolder performingAnimations = new AnimationsHolder();
        [SerializeField, TabGroup("Receiving Animations")]
        private AnimationsHolder receivingAnimations = new AnimationsHolder();


        public override IEnumerator<float> _DoPerformSkillAnimation(SkillValuesHolders skillValues)
        {
            throw new System.NotImplementedException();
        }

        public override void _DoReceiveSkillAnimation(SkillValuesHolders skillValues)
        {
            throw new System.NotImplementedException();
        }

        [Serializable]
        private class
            AnimationsHolder : SerializableSkillInteractionsStructure<List<AnimationClip>, List<AnimationClip>>
        {
            public AnimationClip GetAnimationClip(EnumStats.OffensiveStatType type)
            {
                var clips = UtilStats.GetElement(this, type);
                if (clips.Count <= 0)
                    clips = Offensive;
                return HandleAnimationClips(clips);
            }
            public AnimationClip GetAnimationClip(EnumStats.SupportStatType type)
            {
                var clips = UtilStats.GetElement(this, type);
                if (clips.Count <= 0)
                    clips = Support;
                return HandleAnimationClips(clips);
            }

            private AnimationClip HandleAnimationClips(List<AnimationClip> clips)
            {
                return clips[Random.Range(0, clips.Count)];
            }
        }
    }
}
