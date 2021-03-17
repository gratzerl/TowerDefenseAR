using Assets.Scripts.Logic;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Component for the pause button. 
    /// Registers itself as <see cref="ReferenceableComponent"/> and sets 
    /// the <see cref="VisiblegameStates"/> to <see cref="GameState.Running"/>.
    /// </summary>
    public class PauseButtonHandler : UiElement
    {
        private readonly GameState[] visiblegameStates = new GameState[] { GameState.Running };

        public override GameState[] VisibleGameStates => visiblegameStates;

        protected override void Awake()
        {
            base.Awake();

            gameObject.AddComponent(typeof(ReferenceableComponent));
        }
    }
}
