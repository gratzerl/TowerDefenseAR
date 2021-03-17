using UnityEngine;

namespace Assets.Scripts.Logic
{
    /// <summary>
    /// Bootstrapper for initialising the dependency injection container.
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Called before awake in the MonoBehaviour objects.
        /// Used to register necessary services in the service container.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitializeServices()
        {
            var serviceManager = ServiceContainer.Instance;
            serviceManager.Register<IGameStateService>(new GameStateService());
            serviceManager.Register<IPathGenerator>(new PathGenerator());
            serviceManager.Register<ReferencablesContainer>(new ReferencablesContainer());
            Debug.Log("Initialized services");
        }

        /// <summary>
        /// Called after awake in the MonoBehaviour objects.
        /// Finishes the game initialization.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void FinishInitialization()
        {
            var gameStateService = ServiceContainer.Instance.Get<IGameStateService>();
            gameStateService.InitialiseGame();
            Debug.Log("Finished initialization");
        }
    }
}
