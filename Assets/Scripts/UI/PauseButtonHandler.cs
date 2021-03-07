using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Component for the pause button. 
    /// Registers itself as <see cref="ReferenceableComponent"/> and sets the <see cref="VisiblegameStates"/> to <see cref="GameState.Running"/>.
    /// </summary>
    public class PauseButtonHandler : MonoBehaviour, IUiElement
    {
        private readonly GameState[] visiblegameStates = new GameState[] { GameState.Running };
        public GameState[] VisibleGameStates => visiblegameStates;

        private void Awake()
        {
            gameObject.AddComponent(typeof(ReferenceableComponent));
        }
    }
}
