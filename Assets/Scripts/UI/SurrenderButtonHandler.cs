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
        public override GameState[] VisibleGameStates => visiblegameStates;
        private readonly GameState[] visiblegameStates = new GameState[] { GameState.Paused };

        #region UnityMethods
        protected override void Awake()
        {
            base.Awake();

            gameObject.AddComponent(typeof(ReferenceableComponent));
        }
        #endregion
    }
}
