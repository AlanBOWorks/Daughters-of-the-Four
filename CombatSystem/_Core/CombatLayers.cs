namespace CombatSystem._Core
{
    public static class CombatRenderLayers
    {
        public const int CombatBackgroundIndex = 10;
        public const int CombatLayerBackIndex = CombatBackgroundIndex + 1;
        public const int CombatCharacterBackIndex = CombatLayerBackIndex + 1;
        public const int CombatCharacterFrontIndex = CombatCharacterBackIndex + 1;
        public const int CombatLayerFrontIndex = CombatCharacterFrontIndex + 1;

        public enum EnumLayers
        {
            CombatBackground = CombatBackgroundIndex,
            CombatLayerBack = CombatLayerBackIndex,
            CombatCharacterBack = CombatCharacterBackIndex,
            CombatCharacterFront = CombatCharacterFrontIndex,
            CombatLayerFront = CombatLayerFrontIndex,
        }
    }

    public static class CombatAnimationLayers
    {
        public const int BaseLayerIndex = 0;
        public const int ActionsLayerIndex = BaseLayerIndex + 1;

        public enum EnumLayers
        {
            BaseLayer = BaseLayerIndex,
            ActionsLayer = ActionsLayerIndex
        }
    }

}
