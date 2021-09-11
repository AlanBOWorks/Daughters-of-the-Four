using _CombatSystem;
using Characters;

namespace _Player
{
    public sealed class PlayerUIDictionary : DictionaryEntityElements<PlayerCombatUIElement>
    {
        protected override PlayerCombatUIElement GenerateElement()
            => new PlayerCombatUIElement();
    }

    /// <summary>
    /// It's an element that shows in the combat and
    /// is exclusive to the player (generally UI elements). <br></br><br></br>
    /// The difference to a [<see cref="CombatingEntity"/>]:<br></br>
    /// [<see cref="CombatingEntity"/>] are required for
    /// the Combat.<br></br>
    /// [<see cref="PlayerCombatUIElement"/>] are related to the Combat (but not required for it) yet
    /// essential to them.</summary>
    public sealed class PlayerCombatUIElement : IEntitySwitchListener
    {
        public UCharacterUIHolder UIHolder;

        public UTargetButton GetTargetButton()
        {
            return UIHolder.TargetButton;
        }

        public void OnEntitySwitch(CombatingEntity entity)
        {
            UIHolder.OnEntitySwitch(entity);
        }
    }
}
