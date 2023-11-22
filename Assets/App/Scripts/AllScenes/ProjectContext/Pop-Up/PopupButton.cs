using App.Scripts.Libs.ObjectPool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.AllScenes.ProjectContext.Pop_Up
{
    public class PopupButton : MonoBehaviour, IPoolable
    {
        [SerializeField] private Button button;
        [SerializeField] private Image image;

        public Button.ButtonClickedEvent onClick
        {
            get => button.onClick;
            set => button.onClick = value;
        }

        public Color color
        {
            get => image.color;
            set => image.color = value;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}