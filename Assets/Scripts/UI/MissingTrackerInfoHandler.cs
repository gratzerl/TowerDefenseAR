using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Component for the info box which is shown when required markers are not being
    /// tracked properly.
    /// Registers itself as <see cref="ReferenceableComponent"/> and sets the <see cref="VisiblegameStates"/> to 
    /// <see cref="GameState.MissingTrackers"/>.
    /// </summary>
    public class MissingTrackerInfoHandler : MonoBehaviour, IUiElement
    {
        public GameState[] VisibleGameStates => visiblegameStates;

        private readonly GameState[] visiblegameStates = new GameState[] { GameState.MissingTrackers };

        private void Awake()
        {
            gameObject.AddComponent(typeof(ReferenceableComponent));
        }
    }
}
