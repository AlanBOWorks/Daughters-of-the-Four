
namespace ExplorationSystem
{

    public interface IExplorationThreatsStructureRead<out T>
    {
        T BasicThreatType { get; }
        T EliteThreatType { get; }
        T BossThreatType { get; }
    }

    public interface IExplorationTypesStructureRead<out T> : IExplorationThreatsStructureRead<T>
    {
        T CombinationType { get; }
        T AwakeningType { get; }
        T TreasureType { get; }
        T ShopType { get; }
    }
}
