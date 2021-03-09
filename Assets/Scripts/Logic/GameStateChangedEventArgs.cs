using System;

namespace Assets.Scripts.Logic
{
    public class GameStateChangedEventArgs: EventArgs
    {
        public GameState PreviousState { get; set; }
        public GameState CurrentState { get; set; }
    }
}
