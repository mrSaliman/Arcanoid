using System;
using System.Collections.Generic;
using App.Scripts.Libs.Architecture.Scenes;

namespace App.Scripts.Libs.Architecture
{
    public class RepositoriesBase
    {
        private Dictionary<Type, Repository> _repositoriesMap;
        private SceneConfig _sceneConfig;

        public RepositoriesBase(SceneConfig sceneConfig)
        {
            _sceneConfig = sceneConfig;
            _repositoriesMap = new Dictionary<Type, Repository>();
        }

        public void CreateAllRepositories()
        {
            _repositoriesMap = _sceneConfig.CreateAllRepositories();
        }


        public void SendOnCreateToAllRepositories()
        {
            var allRepositories = _repositoriesMap.Values;
            foreach (var repository in allRepositories)
            {
                repository.OnCreate();
            }
        }
        
        public void InitializeAllRepositories()
        {
            var allRepositories = _repositoriesMap.Values;
            foreach (var repository in allRepositories)
            {
                repository.Initialize();
            }
        }
        
        public void SendOnStartToAllRepositories()
        {
            var allRepositories = _repositoriesMap.Values;
            foreach (var repository in allRepositories)
            {
                repository.OnStart();
            }
        }

        public T GetRepository<T>() where T : Repository
        {
            var type = typeof(T);
            return (T)_repositoriesMap[type];
        }
    }
}