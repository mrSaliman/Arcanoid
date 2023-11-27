using App.Scripts.Scenes.AllScenes.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.PacksScene.UI
{
    public class PackButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image glow, border, galaxy, background;
        [SerializeField] private ControlledLabel label;
        [SerializeField] private ControlledLabel lockedLabel;
        [SerializeField] private TextMeshProUGUI packProgress;

        public TextMeshProUGUI PackProgress => packProgress;

        [SerializeField] private Color unlockedColor;
        [SerializeField] private Color lockedColor;
        [SerializeField] private Sprite lockedSprite;

        private Sprite _mainSprite;
        private Color _mainColor;

        public ControlledLabel Label => label;
        public ControlledLabel LockedLabel => lockedLabel;

        public Button.ButtonClickedEvent onClick => button.onClick;

        public void setColor(Color value)
        {
            _mainColor = value;
            glow.color = value;
            border.color = value;
        }

        public void setSprite(Sprite value)
        {
            _mainSprite = value;
            galaxy.sprite = value;  
        } 

        public void Lock()
        {
            glow.color = lockedColor;
            border.color = lockedColor;
            background.color = lockedColor;
            galaxy.sprite = lockedSprite;
            label.gameObject.SetActive(false);
            lockedLabel.gameObject.SetActive(true);
            button.interactable = false;
        }

        public void Unlock()
        {
            glow.color = _mainColor;
            border.color = _mainColor;
            background.color = unlockedColor;
            galaxy.sprite = _mainSprite;
            label.gameObject.SetActive(true);
            lockedLabel.gameObject.SetActive(false);
            button.interactable = true;
        }
        
    }
}