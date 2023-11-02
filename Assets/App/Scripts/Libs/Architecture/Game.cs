using System;
using App.Scripts.Libs.Architecture.Scenes;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Libs.Architecture
{
    public static class Game
    {
        public static event Action OnGameInitializedEvent;
        
        public static SceneManagerBase SceneManager { get; private set; }

        public static void Run()
        {
            SceneManager = new SceneManagerExample();
            InitializeGameAsync().Forget();
        }

        private static async UniTask InitializeGameAsync()
        {
            SceneManager.InitScenesMap();
            await SceneManager.LoadCurrentSceneAsync();
            OnGameInitializedEvent?.Invoke();
        }

        public static T GetInteractor<T>() where T : Interactor
        {
            return SceneManager.GetInteractor<T>();
        }
        
        public static T GetRepository<T>() where T : Repository
        {
            return SceneManager.GetRepository<T>();
        }
    }
}