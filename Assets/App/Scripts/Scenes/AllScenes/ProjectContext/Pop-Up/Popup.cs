using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using IPoolable = App.Scripts.Libs.ObjectPool.IPoolable;

namespace App.Scripts.Scenes.AllScenes.ProjectContext.Pop_Up
{
    public class Popup : MonoBehaviour, IPoolable
    {
        [SerializeField]
        private ScrollRect scrollRect;

        [SerializeField] private LayoutGroup contentLayout;

        [SerializeField] private RectTransform mainRectTransform;
        [SerializeField] private Button backButton;
        
        public Button BackButton => backButton;
        public RectTransform ContentContainer => scrollRect.content;
    
        private PopupManager _manager;
    
        private readonly List<IPoolable> _poolableElements = new();
        private readonly List<GameObject> _deletableElements = new();

        public void Construct(PopupManager manager)
        {
            _manager = manager;
        }

        public bool VerticalScroll
        {
            get => scrollRect.vertical;
            set => scrollRect.vertical = value;
        }

        public async UniTaskVoid Fit()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
            contentLayout.Recalculate();
            await UniTask.Yield();
            SetHeight(scrollRect.content.sizeDelta.y - scrollRect.viewport.offsetMax.y +
                      scrollRect.viewport.offsetMin.y + 5);
        }

        public void SetHeight(float height)
        {
            var sizeDelta = mainRectTransform.sizeDelta;
            sizeDelta.y = height;
            mainRectTransform.sizeDelta = sizeDelta;
        }

        public void AddPoolableElement<T>(T element) where T : IPoolable
        {
            _poolableElements.Add(element);
        }
    
        public void AddDeletableElement(GameObject element)
        {
            _deletableElements.Add(element);
        }
    
        public void Activate()
        {
        }
    
        public void Deactivate()
        {
            foreach (var deletableElement in _deletableElements)
            {
                Destroy(deletableElement);
            }
            _deletableElements.Clear();
    
            foreach (var poolableElement in _poolableElements)
            {
                _manager.Return(poolableElement);
            }
            _poolableElements.Clear();
    
            _manager = null;
            backButton.onClick.RemoveAllListeners();
            
            VerticalScroll = true;
            gameObject.SetActive(false);
        }
    }
}