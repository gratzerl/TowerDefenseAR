namespace Assets.Scripts.Logic
{
    public enum GameState
    {
        Unknown = 0,
        MissingTrackers,
        Ready,
        Running,
        GameOver,
        Won,
        Paused
    }
}
