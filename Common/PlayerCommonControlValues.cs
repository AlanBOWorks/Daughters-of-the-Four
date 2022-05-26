using UnityEngine;

namespace Common
{
    public sealed class PlayerCommonControlValues : IPlayerPauseValues
    {
        public bool IsGamePaused { get; internal set; }
        internal GameObject GamePauseReferenceObject { get; set; }
    }

    public sealed class PlayerCommonControlValuesSingleton
    {
        static PlayerCommonControlValuesSingleton()
        {
            Values = new PlayerCommonControlValues();
        }

        public static readonly PlayerCommonControlValues Values;
        public static IPlayerPauseValues GetPauseValues() => Values;
    }

    public interface IPlayerPauseValues
    {
        bool IsGamePaused { get; }
    }
}
