using App.Scripts.Libs.ObjectPool;
using App.Scripts.Scenes.AllScenes.UI;
using UnityEngine;

namespace App.Scripts.Scenes.AllScenes.ProjectContext.Pop_Up
{
    public class PopupLabel : MonoBehaviour, IPoolable
    {
        [SerializeField] private ControlledLabel label;
        public float preferredHeight;
        private string _key;

        private LabelController _labelController;
        
        public Color color
        {
            get => label.color;
            set
            {
                label.color = value;
                label.enableVertexGradient = false;
            }
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

        public void SetPreferredHeight()
        {
            var rectTransform = (RectTransform)transform;
            var sizeDelta = rectTransform.sizeDelta;
            sizeDelta.y = preferredHeight;
            rectTransform.sizeDelta = sizeDelta;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            _labelController.RemoveLabel(_key, label);
            label.color = Color.white;
            label.enableVertexGradient = true;
            var rectTransform = (RectTransform)transform;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}