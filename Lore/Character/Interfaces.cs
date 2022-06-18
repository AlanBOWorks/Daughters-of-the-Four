using UnityEngine;

namespace Lore.Character
{
    public interface ICharacterNameStructureRead<out T>
    {
        T CharacterNameType { get; }
        T CharacterFullNameType { get; }
        T CharacterShorterNameType { get; }
    }
    public interface ICharacterNameStructureRead : ICharacterNameStructureRead<string>
    { }



}
