using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

namespace SCharacterCreator.Bones
{
    public interface IHumanoidStructure<T> :
        IBasicHumanoidStructure<T>,
        IHeadStructure<T>, ISpineStructure<T>
    {
        new T HeadType { get; set; }

        IMirrorHumanoidStructureRead<IArmStructure<T>> ArmsType { get; }
        IMirrorHumanoidStructureRead<ILegStructure<T>> LegsType { get; }
    }
    public interface IHumanoidRootsStructure<T> : IHumanoidRootsStructureRead<T>, IHumanoidRootsStructureWrite<T>
    {
        new T BaseRootType { get; set; }
        new T PivotRootType { get; set; }
        new T HeadRootType { get; set; }
    }


    public interface IBasicHumanoidStructure<T> : IBasicHumanoidStructureRead<T>, IBasicHumanoidStructureWrite<T>
    {
        new T HeadType { get; set; }
        new T LeftHandType { get; set; }
        new T RightHandType { get; set; }
        new T LeftFootType { get; set; }
        new T RightFootType { get; set; }
    }

    public interface IMirrorHumanoidStructure<T> : IMirrorHumanoidStructureRead<T>, IMirrorHumanoidStructureWrite<T>
    {
        new T LeftSectionType { get; set; }
        new T RightSectionType { get; set; }
    }

    public interface IHeadStructure<T> : IHeadStructureRead<T>, IHeadStructureWrite<T>
    {
        new T HeadType { get; set; }
        new T NeckType { get; set; }
    }

    public interface ISpineStructure<T> : ISpineStructureRead<T>, ISpineStructureWrite<T>
    {
        new IList<T> SpineTypes { get; }
        new T PelvisType { get; set; }
    }

    public interface IArmStructure<T> : IArmStructureRead<T>, IArmStructureWrite<T>
    {
        new T ShoulderType { get; set; }
        new T UpperArmType { get; set; }
        new T LowerArmType { get; set; }
        new T HandType { get; set; }
    }

    public interface ILegStructure<T> : ILegStructureRead<T>, ILegStructureWrite<T>
    {
        new T ThighType { get; set; }
        new T CalfType { get; set; }
        new T FootType { get; set; }
    }
    public interface IFingersStructure<TRoot, TFinger> : IFingersStructureRead<TRoot, TFinger>, IFingersStructureWrite<TRoot, TFinger>
    {
        new TRoot FingersRootType { get; set; }
        new IList<TFinger> ThumbTypes { get; }
        new IList<TFinger> IndexTypes { get; }
        new IList<TFinger> MiddleTypes { get; }
        new IList<TFinger> RingTypes { get; }
        new IList<TFinger> SmallTypes { get; }
    }


    // ------- GETTERS
    public interface IHumanoidStructureRead<out T> :
        IBasicHumanoidStructureRead<T>,
        IHeadStructureRead<T>, ISpineStructureRead<T>
    {
        new T HeadType { get; }

        IMirrorHumanoidStructureRead<IArmStructureRead<T>> ArmsType { get; }
        IMirrorHumanoidStructureRead<ILegStructureRead<T>> LegsType { get; }
    }
    public interface IHumanoidRootsStructureRead<out T>
    {
        T BaseRootType { get; }
        T PivotRootType { get; }
        T HeadRootType { get; }
    }


    public interface IBasicHumanoidStructureRead<out T>
    {
        T HeadType { get; }
        T LeftHandType { get; }
        T RightHandType { get; }
        T LeftFootType { get; }
        T RightFootType { get; }
    }


    public interface IMirrorHumanoidStructureRead<out T>
    {
        T LeftSectionType { get; }
        T RightSectionType { get; }
    }
    
    public interface IHeadStructureRead<out T>
    {
        T HeadType { get; }
        T NeckType { get; }
    }

    public interface ISpineStructureRead<out T>
    {
        IReadOnlyList<T> SpineTypes { get; }
        T PelvisType { get; }
    }

    public interface IArmStructureRead<out T>
    {
        T ShoulderType { get; }
        T UpperArmType { get; }
        T LowerArmType { get; }
        T HandType { get; }
    }

    public interface ILegStructureRead<out T>
    {
        T ThighType { get; }
        T CalfType { get; }
        T FootType { get; }
    }

    public interface IFingersStructureRead<out TRoot,out TFinger>
    {
        TRoot FingersRootType { get; }
        IReadOnlyList<TFinger> ThumbTypes { get; }
        IReadOnlyList<TFinger> IndexTypes { get; }
        IReadOnlyList<TFinger> MiddleTypes { get; }
        IReadOnlyList<TFinger> RingTypes { get; }
        IReadOnlyList<TFinger> SmallTypes { get; }
    }




    // ------- WRITERS
    public interface IHumanoidStructureWrite<T> :
        IBasicHumanoidStructureWrite<T>,
        IHeadStructureWrite<T>, ISpineStructureWrite<T>
    {
        new T HeadType { set; }

        IMirrorHumanoidStructureWrite<IArmStructureWrite<T>> ArmsType { set; }
        IMirrorHumanoidStructureWrite<ILegStructureWrite<T>> LegsType { set; }
    }
    public interface IHumanoidRootsStructureWrite<in T>
    {
        T BaseRootType { set; }
        T PivotRootType { set; }
        T HeadRootType { set; }
    }


    public interface IBasicHumanoidStructureWrite<in T>
    {
        T HeadType { set; }
        T LeftHandType { set; }
        T RightHandType { set; }
        T LeftFootType { set; }
        T RightFootType { set; }
    }


    public interface IMirrorHumanoidStructureWrite<in T>
    {
        T LeftSectionType { set; }
        T RightSectionType { set; }
    }

    public interface IHeadStructureWrite<in T>
    {
        T HeadType { set; }
        T NeckType { set; }
    }

    public interface ISpineStructureWrite<T>
    {
        IList<T> SpineTypes { get; }
        T PelvisType { set; }
    }

    public interface IArmStructureWrite<in T>
    {
        T ShoulderType { set; }
        T UpperArmType { set; }
        T LowerArmType { set; }
        T HandType { set; }
    }

    public interface ILegStructureWrite<in T>
    {
        T ThighType { set; }
        T CalfType { set; }
        T FootType { set; }
    }

    public interface IFingersStructureWrite<in TRoot, TFinger>
    {
        TRoot FingersRootType { set; }
        IList<TFinger> ThumbTypes { get; }
        IList<TFinger> IndexTypes { get; }
        IList<TFinger> MiddleTypes { get; }
        IList<TFinger> RingTypes { get; }
        IList<TFinger> SmallTypes { get; }
    }
}
