using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Component for logo UI element. 
    /// Registers itself as <see cref="ReferenceableComponent"/> and sets the <see cref="VisiblegameStates"/> to 
    /// <see cref="GameState.GameOver"/>, <see cref="GameState.MissingTrackers"/>, <see cref="GameState.Won"/>, and <see cref="GameState.Ready"/>.
    /// </summary>
    public class LogoHandler : MonoBehaviour, IUiElement
    {
        public GameState[] VisibleGameStates => visiblegameStates;

        private readonly GameState[] visiblegameStates = new GameState[] { GameState.Ready, GameState.Initialised, GameState.MissingTrackers };

        private void Awake()
        {
            gameObject.AddComponent(typeof(ReferenceableComponent));
        }
    }
}
