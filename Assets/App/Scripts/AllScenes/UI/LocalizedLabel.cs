using TMPro;
using UnityEngine;

namespace App.Scripts.AllScenes.UI
{
    public class LocalizedLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;

        private string _text, _data;

        public void SetText(string text)
        {
            _text = text;
            UpdateLabel();
        }

        public void SetData(string data)
        {
            _data = data;
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            label.text = _text + _data;
        }

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
    }
}