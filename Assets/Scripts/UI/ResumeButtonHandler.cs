using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Component for the resume button. 
    /// Registers itself as <see cref="ReferenceableComponent"/> and sets the <see cref="VisiblegameStates"/> to <see cref="GameState.Paused"/>.
    /// </summary>
    public class ResumeButtonHandler : MonoBehaviour, IUiElement
    {
        private readonly GameState[] visiblegameStates = new GameState[] { GameState.Paused };
        public GameState[] VisibleGameStates => visiblegameStates;
        private void Awake()
        {
            gameObject.AddComponent(typeof(ReferenceableComponent));
        }
    }
}