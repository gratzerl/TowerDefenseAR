using Assets.Scripts.Logic;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Component for logo UI element. 
    /// Registers itself as <see cref="ReferenceableComponent"/> and sets the <see cref="VisiblegameStates"/> to 
    /// <see cref="GameState.GameOver"/>, <see cref="GameState.MissingTrackers"/>, and <see cref="GameState.Initialised"/>.
    /// </summary>
    public class LogoHandler : UiElement
    {
        private readonly GameState[] visiblegameStates = new GameState[] { GameState.Ready, GameState.Initialised, GameState.MissingTrackers };

        public override GameState[] VisibleGameStates => visiblegameStates;

        protected override void Awake()
        {
            base.Awake();

            gameObject.AddComponent(typeof(ReferenceableComponent));
        }
    }
}
