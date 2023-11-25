using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.PacksScene.UI
{
    public class PackButton : MonoBehaviour
    {
        [SerializeField] private Button button;

        [SerializeField] private Image glow, border, galaxy;

        public Button Button => button;

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