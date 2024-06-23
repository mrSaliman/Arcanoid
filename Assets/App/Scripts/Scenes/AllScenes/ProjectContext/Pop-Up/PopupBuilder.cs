﻿using App.Scripts.Scenes.AllScenes.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace App.Scripts.Scenes.AllScenes.ProjectContext.Pop_Up
{
    public class PopupBuilder
    {
        private readonly PopupManager _popupManager;
        private readonly Popup _popup;
        private readonly RectTransform _contentContainer;

        public bool Fit = false,
            ActivateBackButton = true,
            IsFadable = false;

        public bool VerticalScroll
        {
            get => _popup.VerticalScroll;
            set => _popup.VerticalScroll = value;
        }

        public PopupBuilder(PopupManager popupManager)
        {
            _popupManager = popupManager;
            _popup = _popupManager.Get<Popup>();
            _contentContainer = _popup.ContentContainer;
            _popup.Construct(_popupManager);
        }

        public void AddBackAction(UnityAction onClick)
        {
            _popup.BackButton.onClick.AddListener(onClick);
        }

        public UnityAction GetBackButtonInvocation => () => _popup.BackButton.onClick.Invoke();

        public PopupBuilder AddLabel(LabelController labelController, string key, float fontSize, Color color)
        {
            if (_contentContainer == null) return this;

            var label = _popupManager.Get<PopupLabel>();

            label.Construct(key, labelController);
            label.color = color;
            label.fontSize = fontSize;
            label.transform.SetParent(_contentContainer, false);
            label.SetPreferredHeight();

            _popup.AddPoolableElement(label);

            return this;
        }

        public PopupBuilder AddButton(LabelController labelController, string key, float fontSize, Color color,
            UnityAction onClick)
        {
            if (_contentContainer == null) return this;

            var button = _popupManager.Get<PopupButton>();
            var label = _popupManager.Get<PopupLabel>();

            label.Construct(key, labelController);
            label.fontSize = fontSize;
            label.transform.SetParent(button.transform, false);

            button.onClick.AddListener(onClick);
            button.color = color;
            button.transform.SetParent(_contentContainer, false);

            _popup.AddPoolableElement(label);
            _popup.AddPoolableElement(button);

            return this;
        }

        public PopupBuilder AddUIElement(GameObject uiElement, UnityAction deleteAction)
        {
            if (_contentContainer == null) return this;

            uiElement.transform.SetParent(_contentContainer, false);
            _popup.AddDeletableElement(deleteAction);

            return this;
        }

        public Popup Build()
        {
            if (Fit) _popup.Fit().Forget();
            /*if (IsFadable)
            {
                _popup.BackButton.onClick.AddListener(UniTask.UnityAction(async () => await _popupManager.Fade(_popup)));
            }*/
            if (ActivateBackButton)
                _popup.BackButton.onClick.AddListener(UniTask.UnityAction(async () =>
                {
                    if (IsFadable)
                    {
                        await _popupManager.Fade(_popup);
                    }

                    _popupManager.Return(_popup);
                    _popupManager.Remove(_popup);
                }));
            else if (IsFadable)
            {
                _popup.BackButton.onClick.AddListener(UniTask.UnityAction(async () =>
                    await _popupManager.Fade(_popup)));
            }
            
            //TODO Move fading to pop up manager

            _popupManager.AddPopup(_popup);
            return _popup;
        }
    }
}