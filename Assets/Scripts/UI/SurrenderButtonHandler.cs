using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Component for the pause button. 
    /// Registers itself as <see cref="ReferenceableComponent"/> and sets the <see cref="VisiblegameStates"/> to <see cref="GameState.Paused"/>.
    /// </summary>
    public class SurrenderButtonHandler : MonoBehaviour, IUiElement
    {
        public GameState[] VisibleGameStates => visiblegameStates;
        private readonly GameState[] visiblegameStates = new GameState[] { GameState.Paused };

        private void Awake()
        {
            gameObject.AddComponent(typeof(ReferenceableComponent));
        }
    }
}
