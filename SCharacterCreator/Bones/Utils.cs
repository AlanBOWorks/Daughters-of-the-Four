using System.Collections.Generic;
using UnityEngine;

namespace SCharacterCreator.Bones
{
    public static class UtilsHumanoid
    {
        public static IEnumerator<T> GetEnumerator<T>(IBasicHumanoidStructureRead<T> structure)
        {
            yield return structure.HeadType;
            yield return structure.LeftHandType;
            yield return structure.RightHandType;
            yield return structure.LeftFootType;
            yield return structure.RightFootType;
        }
    }



    public static class UtilsBones
    {
        public static void InjectBones(HumanoidBonesHolder bonesHolder, Animator animator)
        {
            InjectBones(bonesHolder as IHumanoidStructure<Transform>, animator);
            InjectHandFingersBones(bonesHolder, animator);
        }
        public static void InjectBones(IHumanoidStructure<Transform> structure, Animator animator)
        {
            InjectBasicHumanoidBones(structure,animator);
            InjectHeadBones(structure, animator);
            InjectSpineBones(structure,animator);
            InjectArmsBones(structure.ArmsType,animator);
            InjectLegsBones(structure.LegsType, animator);
        }

        public static void InjectBasicHumanoidBones(IBasicHumanoidStructure<Transform> structure, Animator animator)
        {
            structure.HeadType = animator.GetBoneTransform(HumanBodyBones.Head);
            structure.LeftHandType = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            structure.RightHandType = animator.GetBoneTransform(HumanBodyBones.RightHand);
            structure.LeftFootType = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            structure.RightFootType = animator.GetBoneTransform(HumanBodyBones.RightFoot);
        }

        public static void InjectHeadBones(IHeadStructure<Transform> structure, Animator animator)
        {
            structure.HeadType = animator.GetBoneTransform(HumanBodyBones.Head);
            structure.NeckType = animator.GetBoneTransform(HumanBodyBones.Neck);
        }

        public static void InjectSpineBones(ISpineStructure<Transform> structure, Animator animator)
        {
            structure.PelvisType = animator.GetBoneTransform(HumanBodyBones.Hips);
            var spineEnumerable = UtilsAnimator.GetSpineEnumerable(animator);
            InjectBones(structure.SpineTypes,spineEnumerable);
        }


        public static void InjectArmsBones(IMirrorHumanoidStructureRead<IArmStructure<Transform>> structure, Animator animator)
        {
            var leftShoulder = animator.GetBoneTransform(HumanBodyBones.LeftShoulder);
            var leftUpper = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            var leftLower = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            var leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            InjectArmBones(structure.LeftSectionType,
                leftShoulder, 
                leftUpper,
                leftLower,
                leftHand);

            var rightShoulder = animator.GetBoneTransform(HumanBodyBones.RightShoulder);
            var rightUpper = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
            var rightLower = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
            var rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
            InjectArmBones(structure.RightSectionType,
                rightShoulder,
                rightUpper,
                rightLower,
                rightHand);
        }
        private static void InjectArmBones(IArmStructure<Transform> holder, 
            Transform shoulder, Transform upperArm, Transform lowerArm, Transform hand)
        {
            holder.ShoulderType = shoulder;
            holder.UpperArmType = upperArm;
            holder.LowerArmType = lowerArm;
            holder.HandType = hand;
        }

        public static void InjectLegsBones(IMirrorHumanoidStructureRead<ILegStructure<Transform>> structure,
            Animator animator)
        {
            var leftThigh = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
            var leftCalf = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
            var leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            InjectLegBones(structure.LeftSectionType,leftThigh, leftCalf, leftFoot);

            var rightThigh = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
            var rightCalf = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
            var rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
            InjectLegBones(structure.RightSectionType,rightThigh, rightCalf, rightFoot);
        }

        private static void InjectLegBones(ILegStructure<Transform> holder, 
            Transform thigh, Transform calf, Transform foot)
        {
            holder.ThighType = thigh;
            holder.CalfType = calf;
            holder.FootType = foot;
        }

        public static void InjectHandFingersBones(
            IMirrorHumanoidStructureRead<IFingersStructure<Transform,Transform>> structure,
            Animator animator)
        {
            var leftHolder = structure.LeftSectionType;
            var rightHolder = structure.RightSectionType;
            HandleHolder(leftHolder, true);
            HandleHolder(rightHolder, true);

            void HandleHolder(IFingersStructure<Transform,Transform> holder, bool isLeft)
            {
                var thumbEnumerable = UtilsAnimator.GetThumbsEnumerable(animator, isLeft);
                var indexEnumerable = UtilsAnimator.GetIndexEnumerable(animator, isLeft);
                var middleEnumerable = UtilsAnimator.GetMiddleEnumerable(animator, isLeft);
                var ringEnumerable = UtilsAnimator.GetRingEnumerable(animator, isLeft);
                var littleEnumerable = UtilsAnimator.GetLittleEnumerable(animator, isLeft);
                var rootType = isLeft
                    ? animator.GetBoneTransform(HumanBodyBones.LeftHand)
                    : animator.GetBoneTransform(HumanBodyBones.RightHand);

                holder.FingersRootType = rootType;
                InjectBones(holder.ThumbTypes,thumbEnumerable);
                InjectBones(holder.IndexTypes,indexEnumerable);
                InjectBones(holder.MiddleTypes,middleEnumerable);
                InjectBones(holder.RingTypes, ringEnumerable);
                InjectBones(holder.SmallTypes,littleEnumerable);
            }
        }

        private static void InjectBones(IList<Transform> holder, IEnumerable<Transform> bones)
        {
            if (holder.GetType().IsArray)
            {
                int i = 0;
                foreach (var bone in bones)
                {
                    holder[i] = bone;
                    i++;
                    if (i > holder.Count) return;
                }

                return;
            }

            holder.Clear();
            foreach (var bone in bones)
            {
                holder.Add(bone);
            }
        }
    }

    public static class UtilsAnimator
    {
        public static IEnumerator<Transform> GetBasicAnimatorHumanoid(Animator animator)
        {
            yield return animator.GetBoneTransform(HumanBodyBones.Head);
            yield return animator.GetBoneTransform(HumanBodyBones.LeftHand);
            yield return animator.GetBoneTransform(HumanBodyBones.RightHand);
            yield return animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            yield return animator.GetBoneTransform(HumanBodyBones.RightFoot);
        }

        public static IEnumerable<Transform> GetSpineEnumerable(Animator animator)
        {
            yield return animator.GetBoneTransform(HumanBodyBones.Spine);
            yield return animator.GetBoneTransform(HumanBodyBones.Chest);
            yield return animator.GetBoneTransform(HumanBodyBones.UpperChest);
        }

        public static IEnumerable<Transform> GetArmEnumerable(Animator animator, bool isLeft)
        {
            if (isLeft)
            {
                yield return animator.GetBoneTransform(HumanBodyBones.LeftShoulder);
                yield return animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
                yield return animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
                yield return animator.GetBoneTransform(HumanBodyBones.LeftHand);
            }
            else
            {
                yield return animator.GetBoneTransform(HumanBodyBones.RightShoulder);
                yield return animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
                yield return animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
                yield return animator.GetBoneTransform(HumanBodyBones.RightHand);
            }
        }

        public static IEnumerable<Transform> GetThumbsEnumerable(Animator animator, bool isLeft)
        {
            if (isLeft)
            {
                yield return animator.GetBoneTransform(HumanBodyBones.LeftThumbProximal);
                yield return animator.GetBoneTransform(HumanBodyBones.LeftThumbIntermediate);
                yield return animator.GetBoneTransform(HumanBodyBones.LeftThumbDistal);
            }
            else
            {
                yield return animator.GetBoneTransform(HumanBodyBones.RightThumbProximal);
                yield return animator.GetBoneTransform(HumanBodyBones.RightThumbIntermediate);
                yield return animator.GetBoneTransform(HumanBodyBones.RightThumbDistal);
            }
        }
        public static IEnumerable<Transform> GetIndexEnumerable(Animator animator, bool isLeft)
        {
            if (isLeft)
            {
                yield return animator.GetBoneTransform(HumanBodyBones.LeftIndexProximal);
                yield return animator.GetBoneTransform(HumanBodyBones.LeftIndexIntermediate);
                yield return animator.GetBoneTransform(HumanBodyBones.LeftIndexDistal);
            }
            else
            {
                yield return animator.GetBoneTransform(HumanBodyBones.RightIndexProximal);
                yield return animator.GetBoneTransform(HumanBodyBones.RightIndexIntermediate);
                yield return animator.GetBoneTransform(HumanBodyBones.RightIndexDistal);
            }
        }
        public static IEnumerable<Transform> GetMiddleEnumerable(Animator animator, bool isLeft)
        {
            if (isLeft)
            {
                yield return animator.GetBoneTransform(HumanBodyBones.LeftMiddleProximal);
                yield return animator.GetBoneTransform(HumanBodyBones.LeftMiddleIntermediate);
                yield return animator.GetBoneTransform(HumanBodyBones.LeftMiddleDistal);
            }
            else
            {
                yield return animator.GetBoneTransform(HumanBodyBones.RightMiddleProximal);
                yield return animator.GetBoneTransform(HumanBodyBones.RightMiddleIntermediate);
                yield return animator.GetBoneTransform(HumanBodyBones.RightMiddleDistal);
            }
        }
        public static IEnumerable<Transform> GetRingEnumerable(Animator animator, bool isLeft)
        {
            if (isLeft)
            {
                yield return animator.GetBoneTransform(HumanBodyBones.LeftRingProximal);
                yield return animator.GetBoneTransform(HumanBodyBones.LeftRingIntermediate);
                yield return animator.GetBoneTransform(HumanBodyBones.LeftRingDistal);
            }
            else
            {
                yield return animator.GetBoneTransform(HumanBodyBones.RightRingProximal);
                yield return animator.GetBoneTransform(HumanBodyBones.RightRingIntermediate);
                yield return animator.GetBoneTransform(HumanBodyBones.RightRingDistal);
            }
        }
        public static IEnumerable<Transform> GetLittleEnumerable(Animator animator, bool isLeft)
        {
            if (isLeft)
            {
                yield return animator.GetBoneTransform(HumanBodyBones.LeftLittleProximal);
                yield return animator.GetBoneTransform(HumanBodyBones.LeftLittleIntermediate);
                yield return animator.GetBoneTransform(HumanBodyBones.LeftLittleDistal);
            }
            else
            {
                yield return animator.GetBoneTransform(HumanBodyBones.RightLittleProximal);
                yield return animator.GetBoneTransform(HumanBodyBones.RightLittleIntermediate);
                yield return animator.GetBoneTransform(HumanBodyBones.RightLittleDistal);
            }
        }
    }
}

