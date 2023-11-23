using App.Scripts.AllScenes.ProjectContext;
using App.Scripts.AllScenes.ProjectContext.Pop_Up;
using App.Scripts.AllScenes.UI;
using App.Scripts.GameScene.Game;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.MainMenu.UI
{
    public class MainMenuUIController : MonoBehaviour
    {
        [SerializeField] private Button languageButton, playButton;
        [SerializeField] private RectTransform canvas;

        private PopupManager _popupManager;
        private LabelController _labelController;
        private LocalizationManager _localizationManager;

        [GameInject]
        public void Construct(PopupManager popupManager, LabelController labelController, LocalizationManager localizationManager)
        {
            _popupManager = popupManager;
            _labelController = labelController;
            _localizationManager = localizationManager;
        }
        
        [GameInit]
        public void Init()
        {
            languageButton.onClick.AddListener(HandleLanguageButtonClicked);
            playButton.onClick.AddListener(HandlePlayButtonClicked);
        }

        private void HandleLanguageButtonClicked()
        {
            var builder = new PopupBuilder(_popupManager)
            {
                VerticalScroll = true,
                Fit = true,
                ActivateBackButton = true
            };

            foreach (var language in _localizationManager.LocalizationSettings.AvailableLanguages)
            {
                builder.AddButton(_labelController, language.Language.ToString(), 40, new Color(157, 88, 255),
                    () => _localizationManager.ChangeLanguage(language.Language));
            }
            var popup = builder.Build();
            popup.transform.SetParent(canvas, false);
            popup.gameObject.SetActive(true);
        }

        private void HandlePlayButtonClicked()
        {
            
        }
    }
}