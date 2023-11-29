using System;
using System.Collections.Generic;
using App.Scripts.Configs;
using App.Scripts.Libs.ObjectPool;
using App.Scripts.Scenes.AllScenes.UI;
using App.Scripts.Scenes.GameScene.Game;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Scripts.Scenes.AllScenes.ProjectContext.Pop_Up
{
    [Serializable]
    public class PopupManager
    {
        [SerializeField] private PopupManagerSettings settings;
        [SerializeField] private Transform elementsContainer;
        private ObjectPoolManager _poolManager = new();
        private List<Popup> _popups = new();

        [GameInit]
        public void Init()
        {
            _poolManager.AddPool(() => Object.Instantiate(settings.PopupPrefab, elementsContainer));
            _poolManager.AddPool(() => Object.Instantiate(settings.PopupButtonPrefab, elementsContainer));
            _poolManager.AddPool(() => Object.Instantiate(settings.PopupLabelPrefab, elementsContainer));
        }

        public void AddPopup(Popup popup)
        {
            _popups.Add(popup);
        }

        public void CreateLowEnergyPopup(RectTransform canvas, LabelController labelController)
        {
            var builder = new PopupBuilder(this)
            {
                VerticalScroll = true,
                Fit = true,
                ActivateBackButton = true
            };

            builder.AddLabel(labelController, "low-energy", 45, Color.red);
            var popup = builder.Build();
            popup.transform.SetParent(canvas, false);
            popup.gameObject.SetActive(true);
        }

        public void Clean()
        {
            foreach (var popup in _popups)
            {
                Return(popup);
            }
            _popups.Clear();
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