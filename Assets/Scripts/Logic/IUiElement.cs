namespace Assets.Scripts.Logic
{
    /// <summary>
    /// Marker interface for ui elements to make them easier retrievable
    /// in the <see cref="ReferencablesContainer"/>.
    /// Stores the <see cref="GameState"/> when the ui element is visible (active).
    /// </summary>
    public interface IUiElement
    {
        GameState[] VisibleGameStates { get; }
    }
}
