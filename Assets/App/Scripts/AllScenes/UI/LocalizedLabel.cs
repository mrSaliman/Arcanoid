using TMPro;
using UnityEngine;

namespace App.Scripts.AllScenes.UI
{
    public class LocalizedLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private string key;

        public string Key => key;

        public void SetText(string text)
        {
            label.text = text;
        }
    }
}