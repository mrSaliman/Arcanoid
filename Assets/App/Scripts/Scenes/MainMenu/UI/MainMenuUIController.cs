using App.Scripts.Scenes.AllScenes.ProjectContext;
using App.Scripts.Scenes.AllScenes.ProjectContext.Pop_Up;
using App.Scripts.Scenes.AllScenes.UI;
using App.Scripts.Scenes.GameScene.Game;
using App.Scripts.Scenes.MainMenu.Menu;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        private MenuSceneManager _menuSceneManager;

        [GameInject]
        public void Construct(PopupManager popupManager, LabelController labelController,
            LocalizationManager localizationManager, SceneSwitcher sceneSwitcher, MenuSceneManager menuSceneManager)
        {
            _popupManager = popupManager;
            _labelController = labelController;
            _localizationManager = localizationManager;
            _sceneSwitcher = sceneSwitcher;
            _menuSceneManager = menuSceneManager;
        }
        
        [GameInit]
        public void Init()
        {
            languageButton.onClick.AddListener(HandleLanguageButtonClicked);
            playButton.onClick.AddListener(UniTask.UnityAction(HandlePlayButtonClicked));
        }

        private void HandleLanguageButtonClicked()
        {
            var builder = new PopupBuilder(_popupManager)
            {
                VerticalScroll = true,
                Fit = true,
                ActivateBackButton = true,
                IsFadable = true
            };

            foreach (var language in _localizationManager.LocalizationSettings.AvailableLanguages)
            {
                builder.AddButton(_labelController, language.Language.ToString(), 40, new Color(157, 88, 255),
                    () => _localizationManager.ChangeLanguage(language.Language));
            }
            var popup = builder.Build();
            popup.CanvasGroup.interactable = false;
            popup.transform.SetParent(canvas, false);
            popup.CanvasGroup.alpha = 0f;
            popup.gameObject.SetActive(true);
            
            var fade = popup.CanvasGroup.DOFade(1f, 0.5f);
            fade.OnComplete(() =>
            {
                popup.CanvasGroup.interactable = true;
            });
        }

        private async UniTaskVoid HandlePlayButtonClicked()
        {
            _popupManager.Clean();
            _menuSceneManager.Secede();
            await _sceneSwitcher.LoadSceneAsync("PacksScene");
        }
    }
}