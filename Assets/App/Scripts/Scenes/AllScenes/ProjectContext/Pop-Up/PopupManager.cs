using System;
using System.Collections.Generic;
using App.Scripts.Configs;
using App.Scripts.Libs.ObjectPool;
using App.Scripts.Scenes.AllScenes.UI;
using App.Scripts.Scenes.GameScene.Game;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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

        public void Remove(object value)
        {
            ((System.Collections.IList)_popups).Remove(value);
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
                ActivateBackButton = true,
                IsFadable = true
            };

            builder.AddLabel(labelController, "low-energy", 45, Color.red);
            var popup = builder.Build();
            popup.CanvasGroup.interactable = false;
            popup.transform.SetParent(canvas, false);
            popup.CanvasGroup.alpha = 0f;
            popup.gameObject.SetActive(true);
            var animation = popup.CanvasGroup.DOFade(1f, 1f);
            animation.OnComplete(() =>
            {
                popup.CanvasGroup.interactable = true;
            });
        }

        public void Clean()
        {
            foreach (var popup in _popups)
            {
                Return(popup);
                Fade(popup);
            }

            _popups.Clear();
        }

        public T Get<T>() where T : IPoolable
        {
            return _poolManager.Get<T>();
        }

        public async UniTask Fade(Popup popup)
        {
            popup.CanvasGroup.interactable = false;
            await popup.CanvasGroup.DOFade(0f, 1f);
            popup.CanvasGroup.interactable = true;
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