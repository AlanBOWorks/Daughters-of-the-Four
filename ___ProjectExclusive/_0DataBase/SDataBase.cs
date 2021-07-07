using Sirenix.OdinInspector;
using UnityEngine;

namespace ___ProjectExclusive
{
    [CreateAssetMenu(fileName = "DataBase",
        menuName = "Singleton/DataBase")]
    public class SDataBase : ScriptableObject
    {
#if UNITY_EDITOR
        [TabGroup("Player Characters")]
        public PlayerDataBase PlayerDataBase = DataBaseSingleton.Instance.PlayerDataBase;
#endif
    }

    public sealed class DataBaseSingleton
    {
        static DataBaseSingleton() { }

        private DataBaseSingleton()
        {
            PlayerDataBase = new PlayerDataBase();
        }
        public static DataBaseSingleton Instance { get; } = new DataBaseSingleton();

        public PlayerDataBase PlayerDataBase;
    }
}
