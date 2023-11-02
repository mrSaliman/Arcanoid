using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace App.Scripts.Libs.Architecture.Scenes
{
    public abstract class SceneManagerBase
    {

        public event Action<Scene> OnSceneLoadedEvent; 
        
        public Scene Scene { get; private set; }
        public bool IsLoading { get; private set; }

        protected Dictionary<string, SceneConfig> _sceneConfigMap;
        
        public SceneManagerBase()
        {
            _sceneConfigMap = new Dictionary<string, SceneConfig>();
        }

        protected abstract void InitScenesMap();

        public UniTask LoadCurrentSceneAsync()
        {
            if (IsLoading)
                throw new Exception("Scene is loading now");

            var sceneName = SceneManager.GetActiveScene().name;
            return LoadCurrentSceneAsync(_sceneConfigMap[sceneName]);
        }

        private async UniTask LoadCurrentSceneAsync(SceneConfig sceneConfig)
        {
            IsLoading = true;

            await InitializeSceneAsync(sceneConfig);
            
            IsLoading = false;
            OnSceneLoadedEvent?.Invoke(Scene);
        }

        public UniTask LoadNewSceneAsync(string sceneName)
        {
            if (IsLoading)
                throw new Exception("Scene is loading now");

            return LoadNewSceneAsync(_sceneConfigMap[sceneName]);
        }

        private async UniTask LoadNewSceneAsync(SceneConfig sceneConfig)
        {
            IsLoading = true;

            await LoadSceneAsync(sceneConfig);
            await InitializeSceneAsync(sceneConfig);
            
            IsLoading = false;
            OnSceneLoadedEvent?.Invoke(Scene);
        }
        
        private async UniTask LoadSceneAsync(SceneConfig sceneConfig)
        {
            var async = SceneManager.LoadSceneAsync(sceneConfig.SceneName);
            async.allowSceneActivation = false;

            while (async.progress < 0.9)
                await UniTask.Yield();

            async.allowSceneActivation = true;
        }
        
        private async UniTask InitializeSceneAsync(SceneConfig sceneConfig)
        {
            Scene = new Scene(sceneConfig);
            await Scene.InitializeAsync();
        }

        public T GetRepository<T>() where T : Repository
        {
            return Scene.GetRepository<T>();
        }
        
        public T GetInteractor<T>() where T : Interactor
        {
            return Scene.GetInteractor<T>();
        }
    }
}