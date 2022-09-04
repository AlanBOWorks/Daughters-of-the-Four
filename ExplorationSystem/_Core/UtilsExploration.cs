using System.Collections.Generic;

namespace ExplorationSystem
{
    public static class UtilsExploration
    {
        public static IEnumerable<T> GetEnumerable<T>(IExplorationTypesStructureRead<T> structure)
        {
            yield return structure.BasicThreatType;
            yield return structure.EliteThreatType;
            yield return structure.BossThreatType;

            yield return structure.CombinationType;
            yield return structure.AwakeningType;

            yield return structure.TreasureType;
            yield return structure.ShopType;
        }
        public static IEnumerable<KeyValuePair<TKey, TValue>> GetEnumerable<TKey, TValue>
            (IExplorationTypesStructureRead<TKey> keys, IExplorationTypesStructureRead<TValue> values)
        {
            yield return new KeyValuePair<TKey, TValue>(keys.BasicThreatType, values.BasicThreatType);
            yield return new KeyValuePair<TKey, TValue>(keys.EliteThreatType, values.EliteThreatType);
            yield return new KeyValuePair<TKey, TValue>(keys.BossThreatType, values.BossThreatType);

            yield return new KeyValuePair<TKey, TValue>(keys.CombinationType, values.CombinationType);
            yield return new KeyValuePair<TKey, TValue>(keys.AwakeningType, values.AwakeningType);

            yield return new KeyValuePair<TKey, TValue>(keys.TreasureType, values.TreasureType);
            yield return new KeyValuePair<TKey, TValue>(keys.ShopType, values.ShopType);
        }

    }

    public static class UtilsExplorationMechanics
    {
        public static void InvokeExplorationBehaviourType(EnumExploration.ExplorationType type)
        {
            switch (type)
            {
                default:
                    return;
                case EnumExploration.ExplorationType.BasicThreat:
                    InvokeBasicCombat();
                    return;
                case EnumExploration.ExplorationType.EliteThreat:
                    InvokeEliteCombat();
                    return;
                case EnumExploration.ExplorationType.BossThreat:
                    InvokeBossCombat();
                    return;
            }
        }

        public static void InvokeBasicCombat()
        {
            var playerTeam = PlayerExplorationSingleton.GetPlayerTeamProvider();
        }
        public static void InvokeEliteCombat()
        {

        }
        public static void InvokeBossCombat()
        {

        }
    }
}
