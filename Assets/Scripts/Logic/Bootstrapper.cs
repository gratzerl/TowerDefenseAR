using UnityEngine;

namespace Assets.Scripts.Logic
{
    /// <summary>
    /// Bootstrapper for initialising the dependency injection container.
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Called before awake in the monobehaviour objects.
        /// Used to setup necessary services.
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
        /// Called after awake in the monobehaviour objects.
        /// Finishes the game initalisation.
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
