using System;
using System.Collections.Generic;
using CombatSkills;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CombatSystem.Animator
{

    [CreateAssetMenu(fileName = "N [List Animations]",
        menuName = "Combat/Animations/List Animations")]
    public class SCombatAnimationsLists : SCombatAnimationsHolder
    {
        [SerializeField,HorizontalGroup()]
        private AnimationsHolder performActions = new AnimationsHolder();
        [SerializeField, HorizontalGroup()]
        private AnimationsHolder receiveActions = new AnimationsHolder();


        public override AnimationClip GetPerformActionAnimation(SkillValuesHolders skillValues)
        {
            throw new NotImplementedException();
        }

        public override AnimationClip GetReceiveActionAnimation(SkillValuesHolders skillValues)
        {
            throw new NotImplementedException();
        }




        [Serializable]
        private class
            AnimationsHolder : SerializableSkillInteractionsStructure<List<AnimationClip>, List<AnimationClip>>
        {
            public AnimationClip GetAnimationClip(EnumStats.OffensiveStatType type)
            {
                var clips = UtilStats.GetElement(type, this);
                if (clips.Count <= 0)
                    clips = Offensive;
                return HandleAnimationClips(clips);
            }
            public AnimationClip GetAnimationClip(EnumStats.SupportStatType type)
            {
                var clips = UtilStats.GetElement(type, this);
                if (clips.Count <= 0)
                    clips = Support;
                return HandleAnimationClips(clips);
            }
            public AnimationClip GetAnimationClip(EnumSkills.DominionType type)
            {
                var clips = UtilSkills.GetElement(this, type);
                if (clips.Count <= 0)
                    clips = Dominion;
                return HandleAnimationClips(clips);
            }


            private AnimationClip HandleAnimationClips(List<AnimationClip> clips)
            {
                return clips[Random.Range(0, clips.Count)];
            }
        }

    }
}
