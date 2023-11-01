using System;
using System.Collections.Generic;
using App.Scripts.Architecture;

namespace App.Scripts.Scenes
{
    public sealed class SceneConfigExample : SceneConfig
    {
        public const string SCENE_NAME = "Example";

        public override string SceneName => SCENE_NAME;
        
        public override Dictionary<Type, Repository> CreateAllRepositories()
        {
            var repositoriesMap = new Dictionary<Type, Repository>();
            return repositoriesMap;
        }

        public override Dictionary<Type, Interactor> CreateAllInteractors()
        {
            var interactorsMap = new Dictionary<Type, Interactor>();
            return interactorsMap;
        }

    }
}