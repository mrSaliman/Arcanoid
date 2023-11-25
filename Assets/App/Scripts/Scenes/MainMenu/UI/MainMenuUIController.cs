using App.Scripts.Scenes.AllScenes.ProjectContext;
using App.Scripts.Scenes.AllScenes.ProjectContext.Pop_Up;
using App.Scripts.Scenes.AllScenes.UI;
using App.Scripts.Scenes.GameScene.Game;
using App.Scripts.Scenes.MainMenu.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenu.UI
{
    public class MainMenuUIController : MonoBehaviour
    {
        [SerializeField] private Button languageButton, playButton;
        [SerializeField] private RectTransform canvas;

        private PopupManager _popupManager;
        private LabelController _labelController;
        private LocalizationManager _localizationManager;
        private SceneSwitcher _sceneSwitcher;
        private MenuManager _menuManager;

        [GameInject]
        public void Construct(PopupManager popupManager, LabelController labelController,
            LocalizationManager localizationManager, SceneSwitcher sceneSwitcher, MenuManager menuManager)
        {
            _popupManager = popupManager;
            _labelController = labelController;
            _localizationManager = localizationManager;
            _sceneSwitcher = sceneSwitcher;
            _menuManager = menuManager;
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

        private async void HandlePlayButtonClicked()
        {
            _popupManager.Clean();
            _menuManager.Secede();
            await _sceneSwitcher.LoadSceneAsync("PacksScene");
        }
    }
}