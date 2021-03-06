namespace Assets.Scripts.Logic
{
    /// <summary>
    /// Enum for the current state of the game.
    /// </summary>
    public enum GameState
    {
        Unknown = 0,
        Initialised,
        MissingMarkers,
        Ready,
        Running,
        StageCleared,
        GameOver,
        Won,
        Paused
    }
}
