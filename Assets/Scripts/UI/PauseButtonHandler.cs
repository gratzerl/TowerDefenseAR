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
        public override GameState[] VisibleGameStates => visiblegameStates;

        private readonly GameState[] visiblegameStates = new GameState[] { GameState.Running };

        #region UnityMethods
        protected override void Awake()
        {
            base.Awake();

            gameObject.AddComponent(typeof(ReferenceableComponent));
        }
        #endregion
    }
}
