using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SCharacterCreator.Bones
{
    [Serializable]
    public sealed class HumanoidBonesHolder : HumanoidStructure<Transform>,
        IHumanoidBonesHolder, IBasicHumanoidBonesHolder,
        IHeadBonesHolder, ISpineBonesHolder,
        IMirrorHumanoidStructureRead<IFingersBoneHolder>
    {
        [SerializeReference, HorizontalGroup()] private IFingersBoneHolder leftHandFingersBones = new FingersHolder();
        [SerializeReference, HorizontalGroup()] private IFingersBoneHolder rightHandFingersBones = new FingersHolder();
        

        public new IMirrorHumanoidStructureRead<IArmStructureRead<Transform>> ArmsType => this;
        public new IMirrorHumanoidStructureRead<ILegStructureRead<Transform>> LegsType => this;
        public IFingersBoneHolder LeftSectionType => leftHandFingersBones;
        public IFingersBoneHolder RightSectionType => rightHandFingersBones;
    }

    [Serializable]
    public sealed class FingersHolder : FingersStructure<Transform,Transform>, IFingersBoneHolder
    { }

    public interface IBasicHumanoidBonesHolder : IBasicHumanoidStructure<Transform> { }
    public interface IHumanoidBonesHolder : IHumanoidStructure<Transform> { }

    public interface IHeadBonesHolder : IHeadStructure<Transform> { }
    public interface ISpineBonesHolder : ISpineStructure<Transform> { }
    public interface IArmBonesHolder : IArmStructure<Transform> { }
    public interface IArmsBonesHolder : IMirrorHumanoidStructure<IArmBonesHolder> { }
    public interface ILegBonesHolder : ILegStructure<Transform> { }
    public interface ILegsBonesHolder : IMirrorHumanoidStructure<ILegBonesHolder> { }

    public interface IFingersBoneHolder : IFingersStructure<Transform, Transform> { }
}
