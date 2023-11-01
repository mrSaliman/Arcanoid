using System;
using System.Collections.Generic;
using App.Scripts.Scenes;

namespace App.Scripts.Architecture
{
    public class InteractorsBase
    {
        private Dictionary<Type, Interactor> _interactorsMap;
        private SceneConfig _sceneConfig;

        public InteractorsBase(SceneConfig sceneConfig)
        {
            _sceneConfig = sceneConfig;
            _interactorsMap = new Dictionary<Type, Interactor>();
        }

        public void CreateAllInteractors()
        {
            _interactorsMap = _sceneConfig.CreateAllInteractors();
        }
        

        public void SendOnCreateToAllInteractors()
        {
            var allInteractors = _interactorsMap.Values;
            foreach (var interactor in allInteractors)
            {
                interactor.OnCreate();
            }
        }
        
        public void InitializeAllInteractors()
        {
            var allInteractors = _interactorsMap.Values;
            foreach (var interactor in allInteractors)
            {
                interactor.Initialize();
            }
        }
        
        public void SendOnStartToAllInteractors()
        {
            var allInteractors = _interactorsMap.Values;
            foreach (var interactor in allInteractors)
            {
                interactor.OnStart();
            }
        }

        public T GetInteractor<T>() where T : Interactor
        {
            var type = typeof(T);
            return (T)_interactorsMap[type];
        }
    }
}