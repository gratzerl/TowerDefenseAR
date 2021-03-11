namespace Assets.Scripts.Logic
{
    public enum GameState
    {
        Unknown = 0,
        Initialised,
        MissingTrackers,
        Ready,
        Running,
        GameOver,
        Won,
        Paused
    }
}
