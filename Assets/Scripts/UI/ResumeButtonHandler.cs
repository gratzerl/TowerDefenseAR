using Assets.Scripts.Logic;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Component for the resume button. 
    /// Registers itself as <see cref="ReferenceableComponent"/> and sets the <see cref="VisiblegameStates"/> to <see cref="GameState.Paused"/>.
    /// </summary>
    public class ResumeButtonHandler : UiElement
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