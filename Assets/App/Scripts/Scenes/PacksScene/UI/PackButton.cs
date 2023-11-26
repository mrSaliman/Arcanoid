using App.Scripts.Scenes.AllScenes.UI;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.PacksScene.UI
{
    public class PackButton : MonoBehaviour
    {
        [SerializeField] private Button button;

        [SerializeField] private Image glow, border, galaxy;

        [SerializeField] private ControlledLabel label;

        public ControlledLabel Label => label;

        public Button.ButtonClickedEvent onClick
        {
            get => button.onClick;
            set => button.onClick = value;
        }

        public Color color
        {
            get => glow.color;
            set
            {
                glow.color = value;
                border.color = value;
            }
        }

        public Sprite sprite
        {
            get => galaxy.sprite;
            set => galaxy.sprite = value;
        }
    }
}