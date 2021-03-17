using Assets.Scripts.Logic;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Component for the info box which is shown when required markers are not being
    /// tracked properly.
    /// Registers itself as <see cref="ReferenceableComponent"/> and sets the <see cref="VisiblegameStates"/> to 
    /// <see cref="GameState.MissingTrackers"/>.
    /// </summary>
    public class MissingTrackerInfoHandler : UiElement
    {
        private readonly GameState[] visiblegameStates = new GameState[] { GameState.MissingTrackers };

        public override GameState[] VisibleGameStates => visiblegameStates;

        protected override void Awake()
        {
            base.Awake();

            gameObject.AddComponent(typeof(ReferenceableComponent));
        }
    }
}
