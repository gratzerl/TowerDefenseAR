using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Component for the start button. 
    /// Registers itself as <see cref="ReferenceableComponent"/> and sets the <see cref="VisiblegameStates"/> to 
    /// <see cref="GameState.GameOver"/>, <see cref="GameState.Won"/>, and <see cref="GameState.Ready"/>.
    /// </summary>
    public class StartButtonHandler : MonoBehaviour, IUiElement
    {
        public GameState[] VisibleGameStates => visiblegameStates;

        private readonly GameState[] visiblegameStates = new GameState[] { GameState.GameOver, GameState.Ready, GameState.Won };

        private void Awake()
        {
            gameObject.AddComponent(typeof(ReferenceableComponent));
        }
    }
}