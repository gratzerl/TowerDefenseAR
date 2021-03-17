using Assets.Scripts.Logic;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Component for the start button. 
    /// Registers itself as <see cref="ReferenceableComponent"/> and sets the <see cref="VisiblegameStates"/> to 
    /// <see cref="GameState.GameOver"/>, <see cref="GameState.Won"/>, and <see cref="GameState.Ready"/>.
    /// </summary>
    public class StartButtonHandler : UiElement
    {
        public override GameState[] VisibleGameStates => visiblegameStates;

        private readonly GameState[] visiblegameStates = new GameState[] { GameState.GameOver, GameState.Ready, GameState.Won };

        #region UnityMethods
        protected override void Awake()
        {
            base.Awake();

            gameObject.AddComponent(typeof(ReferenceableComponent));
        }
        #endregion
    }
}