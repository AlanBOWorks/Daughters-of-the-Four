using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SCharacterCreator.Bones
{
    [Serializable]
    public class BasicHumanoidStructure<T> : IBasicHumanoidStructure<T>
    {
        public T headType;
        [HorizontalGroup("Left")]
        public T leftHandType;
        [HorizontalGroup("Right")]
        public T rightHandType;
        [HorizontalGroup("Left")]
        public T leftFootType;
        [HorizontalGroup("Right")]
        public T rightFootType;


        public T HeadType
        {
            get => headType;
            set => headType = value;
        }

        public T LeftHandType
        {
            get => leftHandType;
            set => leftHandType = value;
        }

        public T RightHandType
        {
            get => rightHandType;
            set => rightHandType = value;
        }

        public T LeftFootType
        {
            get => leftFootType;
            set => leftFootType = value;
        }

        public T RightFootType
        {
            get => rightFootType;
            set => rightFootType = value;
        }

        protected void SerializeAsNew<TSerializable>(BasicHumanoidStructure<TSerializable> structure) where TSerializable : new()
        {
            structure.headType = new TSerializable();
            structure.leftHandType = new TSerializable();
            structure.rightHandType = new TSerializable();
            structure.leftFootType = new TSerializable();
            structure.rightFootType = new TSerializable();
        }
    }

    [Serializable]
    public class BasicClassHumanoidStructure<T> : BasicHumanoidStructure<T> where T : new()
    {
        public BasicClassHumanoidStructure()
        {
            SerializeAsNew(this);
        }
    }

    [Serializable]
    public class HumanoidStructure<T> :
        IHumanoidStructure<T>,
        IMirrorHumanoidStructureRead<IArmStructure<T>>,
        IMirrorHumanoidStructureRead<ILegStructure<T>>
    {
        public T headType;
        public T neckType;
        public T pelvisType;

        [SerializeField]
        private T[] spineTypes = new T[3];

        [SerializeField, HorizontalGroup("Arms")] private ArmStructure leftArmType;
        [SerializeField, HorizontalGroup("Arms")] private ArmStructure rightArmType;

        [SerializeField, HorizontalGroup("Legs")] private LegStructure leftLegType;
        [SerializeField, HorizontalGroup("Legs")] private LegStructure rightLegType;
       


        public HumanoidStructure()
        {
           leftArmType = new ArmStructure();
           rightArmType = new ArmStructure();
           leftLegType = new LegStructure();
           rightLegType = new LegStructure();
        }

        public T HeadType
        {
            get => headType;
            set => headType = value;
        }

        public T LeftHandType
        {
            get => ArmsType.LeftSectionType.HandType;
            set => ArmsType.LeftSectionType.HandType = value;
        }

        public T RightHandType
        {
            get => ArmsType.RightSectionType.HandType;
            set => ArmsType.RightSectionType.HandType = value;
        }

        public T LeftFootType
        {
            get => LegsType.LeftSectionType.FootType;
            set => LegsType.LeftSectionType.FootType = value;
        }

        public T RightFootType
        {
            get => LegsType.LeftSectionType.FootType;
            set => LegsType.LeftSectionType.FootType = value;
        }

        public T NeckType
        {
            get => neckType;
            set => neckType = value;
        }

        public IList<T> SpineTypes => spineTypes;
        IReadOnlyList<T> ISpineStructureRead<T>.SpineTypes => spineTypes;

        public T PelvisType
        {
            get => pelvisType;
            set => pelvisType = value;
        }

        public IMirrorHumanoidStructureRead<IArmStructure<T>> ArmsType => this;
        public IMirrorHumanoidStructureRead<ILegStructure<T>> LegsType => this;


        protected interface IArmStructure : IArmStructure<T>{}
        protected interface ILegStructure : ILegStructure<T> { }

        IArmStructure<T> IMirrorHumanoidStructureRead<IArmStructure<T>>.LeftSectionType => leftArmType;
        IArmStructure<T> IMirrorHumanoidStructureRead<IArmStructure<T>>.RightSectionType => rightArmType;
        ILegStructure<T> IMirrorHumanoidStructureRead<ILegStructure<T>>.RightSectionType => rightLegType;
        ILegStructure<T> IMirrorHumanoidStructureRead<ILegStructure<T>>.LeftSectionType => leftLegType;

        [Serializable]
        private sealed class ArmStructure : ArmStructure<T>, IArmStructure { }
        [Serializable]
        private sealed class LegStructure : LegStructure<T>, ILegStructure { }

    }



    [Serializable]
    public class HumanoidMirrorStructure<T> : IMirrorHumanoidStructure<T>
    {
        [SerializeField, HorizontalGroup()] protected T leftSectionType;
        [SerializeField, HorizontalGroup()] protected T rightSectionType;

        public T LeftSectionType
        {
            get => leftSectionType;
            set => leftSectionType = value;
        }

        public T RightSectionType
        {
            get => rightSectionType;
            set => rightSectionType = value;
        }
    }
    [Serializable]
    public class ClassHumanoidMirrorStructure<T> : HumanoidMirrorStructure<T> where T : new()
    {
        public ClassHumanoidMirrorStructure()
        {
            leftSectionType = new T();
            rightSectionType = new T();
        }
    }

    [Serializable]
    public class ClassArmStructure<T> : ArmStructure<T> where T : new()
    {
        public ClassArmStructure()
        {
            shoulderType = new T();
            handType = new T();
        }
    }

    [Serializable]
    public class ArmStructure<T> : IArmStructure<T>
    {
        public T shoulderType;

        public T handType;
        public T upperArmType;
        public T lowerArmType;

        public T ShoulderType
        {
            get => shoulderType;
            set => shoulderType = value;
        }

        public T UpperArmType
        {
            get => upperArmType;
            set => upperArmType = value;
        }

        public T LowerArmType
        {
            get => lowerArmType;
            set => lowerArmType = value;
        }

        public T HandType
        {
            get => handType;
            set => handType = value;
        }
    }

    [Serializable]
    public class ClassLegStructure<T> : LegStructure<T> where T : new()
    {
        public ClassLegStructure()
        {
            thighType = new T();
            footType = new T();
        }
    }
    [Serializable]
    public class LegStructure<T> : ILegStructure<T>
    {
        public T thighType;
        public T footType;
        public T calfType;

        public T ThighType
        {
            get => thighType;
            set => thighType = value;
        }

        public T CalfType
        {
            get => calfType;
            set => calfType = value;
        }

        public T FootType
        {
            get => footType;
            set => footType = value;
        }
    }

    [Serializable]
    public class FingersStructure<TRoot,TFinger> : IFingersStructure<TRoot,TFinger>
    {
        public TRoot fingersRootType;
        [SerializeField] private TFinger[] thumbTypes = new TFinger[3];
        [SerializeField] private TFinger[] indexTypes = new TFinger[3];
        [SerializeField] private TFinger[] middleTypes = new TFinger[3];
        [SerializeField] private TFinger[] heartTypes = new TFinger[3];
        [SerializeField] private TFinger[] smallTypes = new TFinger[3];


        public TRoot FingersRootType
        {
            get => fingersRootType;
            set => fingersRootType = value;
        }

        IReadOnlyList<TFinger> IFingersStructureRead<TRoot, TFinger>.ThumbTypes => thumbTypes;
        IReadOnlyList<TFinger> IFingersStructureRead<TRoot, TFinger>.IndexTypes => indexTypes;
        IReadOnlyList<TFinger> IFingersStructureRead<TRoot, TFinger>.MiddleTypes => middleTypes;
        IReadOnlyList<TFinger> IFingersStructureRead<TRoot, TFinger>.RingTypes => heartTypes;
        IReadOnlyList<TFinger> IFingersStructureRead<TRoot, TFinger>.SmallTypes => smallTypes;

        public IList<TFinger> ThumbTypes => thumbTypes;
        public IList<TFinger> IndexTypes => indexTypes;
        public IList<TFinger> MiddleTypes => middleTypes;
        public IList<TFinger> RingTypes => heartTypes;
        public IList<TFinger> SmallTypes => smallTypes;
    }
}
