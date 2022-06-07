using System;
using CombatSystem.Skills;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Animations
{

    [CreateAssetMenu(fileName = "N [Basic Combat Animations]", 
        menuName = "Combat/Animations/Basic Structure")]
    public class SCombatAnimationsBasic : ScriptableObject, IEntityAnimationsPackStructureRead<AnimationClip>
    {
        [Title("Cinematic Clips")]
        [SerializeField] private AnimationClip initialAnimationClip;
        [Title("Actions Clips")]
        [SerializeField] private AnimationClip idle;
        [SerializeField] private AnimationClip activeIdle;

        [SerializeField] private AnimationSet performActionClips = new AnimationSet();
        [SerializeField] private AnimationSet receiveActionClips = new AnimationSet();

        public AnimationClip GetIdleClip() => idle;
        public AnimationClip GetActiveClip() => activeIdle;

        public AnimationClip InitialAnimationType => initialAnimationClip;
        public ISkillArchetypeStructureRead<AnimationClip> AnimationPerformType => performActionClips;
        public ISkillArchetypeStructureRead<AnimationClip> AnimationReceiveType => receiveActionClips;



        [Serializable]
        private sealed class AnimationSet : ISkillArchetypeStructureRead<AnimationClip>
        {
            [Title("Global clip")]
            [SerializeField] private AnimationClip globalClip;
            [Title("Dedicates clips")]
            [SerializeField] private AnimationClip selfType;
            [SerializeField] private AnimationClip offensiveType;
            [SerializeField] private AnimationClip supportType;

            public AnimationClip SelfSkillType => selfType ? selfType : globalClip;
            public AnimationClip OffensiveSkillType => offensiveType ? offensiveType : globalClip;
            public AnimationClip SupportSkillType => supportType ? supportType : globalClip;
        }
    }
}
