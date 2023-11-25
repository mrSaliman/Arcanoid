using App.Scripts.Scenes.AllScenes.UI;
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
            ActivateBackButton = true;

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

        public PopupBuilder AddLabel(LabelController labelController, string key, float fontSize, Color color)
        {
            if (_contentContainer == null) return this;
            
            var label = _popupManager.Get<PopupLabel>();
            
            label.Construct(key, labelController);
            label.color = color;
            label.fontSize = fontSize;
            label.transform.SetParent(_contentContainer, false);
            
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

        public PopupBuilder AddUIElement(GameObject uiElement)
        {
            if (_contentContainer == null) return this;
            
            uiElement.transform.SetParent(_contentContainer, false);
            _popup.AddDeletableElement(uiElement);
            
            return this;
        }

        public Popup Build()
        {
            if (Fit) _popup.Fit();
            if (ActivateBackButton) _popup.BackButton.onClick.AddListener(() => _popupManager.Return(_popup));
            _popupManager.AddPopup(_popup);
            return _popup;
        }
    }
}