using UnityEngine;

namespace SCharacterCreator.Bones
{
    public interface IHumanoidRootsStructureRead<out T>
    {
        T BaseRootType { get; }
        T PivotRootType { get; }
        T HeadRootType { get; }
    }
}
