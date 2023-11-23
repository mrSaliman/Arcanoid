﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IPoolable = App.Scripts.Libs.ObjectPool.IPoolable;

namespace App.Scripts.AllScenes.ProjectContext.Pop_Up
{
    public class Popup : MonoBehaviour, IPoolable
    {
        [SerializeField]
        private ScrollRect scrollRect;
        public RectTransform ContentContainer => scrollRect.content;
    
        private PopupManager _manager;
    
        private readonly List<IPoolable> _poolableElements = new();
        private readonly List<GameObject> _deletableElements = new();

        public void Construct(PopupManager manager)
        {
            _manager = manager;
        }

        public bool vertical
        {
            get => scrollRect.vertical;
            set => scrollRect.vertical = value;
        }

        public void Fit()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
            var rectTransform = (RectTransform)transform;
            var sizeDelta = rectTransform.sizeDelta;
            sizeDelta.y = scrollRect.content.sizeDelta.y - scrollRect.viewport.offsetMax.y +
                          scrollRect.viewport.offsetMin.y + 5;
            rectTransform.sizeDelta = sizeDelta;
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
            
            vertical = true;
            gameObject.SetActive(false);
        }
    }
}