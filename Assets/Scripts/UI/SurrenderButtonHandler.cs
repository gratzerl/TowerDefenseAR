using Assets.Scripts.Logic;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Component for the pause button. 
    /// Registers itself as <see cref="ReferenceableComponent"/> and sets 
    /// the <see cref="VisiblegameStates"/> to <see cref="GameState.Paused"/>.
    /// </summary>
    public class SurrenderButtonHandler : UiElement
    {
        private readonly GameState[] visiblegameStates = new GameState[] { GameState.Paused };
        
        public override GameState[] VisibleGameStates => visiblegameStates;

        protected override void Awake()
        {
            base.Awake();

            gameObject.AddComponent(typeof(ReferenceableComponent));
        }
    }
}
