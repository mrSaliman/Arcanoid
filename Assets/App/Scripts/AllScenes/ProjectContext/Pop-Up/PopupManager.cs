using System;
using App.Scripts.Configs;
using App.Scripts.GameScene.Game;
using App.Scripts.Libs.ObjectPool;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Scripts.AllScenes.ProjectContext.Pop_Up
{
    [Serializable]
    public class PopupManager
    {
        [SerializeField] private PopupManagerSettings settings;
        [SerializeField] private Transform elementsContainer;
        private ObjectPoolManager _poolManager = new();

        [GameInit]
        public void Init()
        {
            _poolManager.AddPool(() => Object.Instantiate(settings.PopupPrefab, elementsContainer));
            _poolManager.AddPool(() => Object.Instantiate(settings.PopupButtonPrefab, elementsContainer));
            _poolManager.AddPool(() => Object.Instantiate(settings.PopupLabelPrefab, elementsContainer));
        }
        
        public T Get<T>() where T : IPoolable
        {
            return _poolManager.Get<T>();
        }

        public void Return<T>(T element) where T : IPoolable
        {
            if (element is Component component) component.transform.SetParent(elementsContainer, false);
            _poolManager.Return(element);
        }

        public void Return(IPoolable element)
        {
            if (element is Component component) component.transform.SetParent(elementsContainer, false);
            _poolManager.Return(element);
        }
    }
}