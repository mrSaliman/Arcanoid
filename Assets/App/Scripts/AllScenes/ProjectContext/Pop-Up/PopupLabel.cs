using App.Scripts.AllScenes.UI;
using App.Scripts.Libs.ObjectPool;
using UnityEngine;

namespace App.Scripts.AllScenes.ProjectContext.Pop_Up
{
    public class PopupLabel : MonoBehaviour, IPoolable
    {
        [SerializeField] private LocalizedLabel label;
        private string _key;

        private LabelController _labelController;
        
        public Color color
        {
            get => label.color;
            set => label.color = value;
        }

        public float fontSize
        {
            get => label.fontSize;
            set => label.fontSize = value;
        }
        
        public void Construct(string key, LabelController labelController)
        {
            _key = key;
            _labelController = labelController;
            _labelController.AddLabel(key, label);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            _labelController.RemoveLabel(_key, label);
            color = Color.white;
            var rectTransform = (RectTransform)transform;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}