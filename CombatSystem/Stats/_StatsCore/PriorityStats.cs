using System.Collections.Generic;

namespace CombatSystem.Stats
{

    public static class UtilsPriorityStats
    {
        public static IEnumerable<T> GetEnumerable<T>(IMainStatsRead<T> stats, EnumStats.MasterStatType type)
        {
            for (int i = 0; i < EnumStats.MasterTypeCount; i++)
            {
                yield return UtilsStats.GetElement(type, stats);
                type++;
                if (type == EnumStats.MasterStatType.Concentration)
                    type = EnumStats.MasterStatType.Offensive;
            }
        }

    }

    public interface IPriorityStatStructureRead<out T>
    {
        T PriorityElementType { get; }
        T SecondPriorityElementType { get; }
        T ThirdPriorityElementType { get; }
        T FourthPriorityElementType { get; }
    }
}
