using App.Scripts.Scenes.AllScenes.ProjectContext;
using App.Scripts.Scenes.AllScenes.ProjectContext.Pop_Up;
using App.Scripts.Scenes.AllScenes.UI;
using App.Scripts.Scenes.GameScene.Game;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.UI
{
    public class GamePopupController : MonoBehaviour
    {
        [SerializeField] private Button menuButton;
        [SerializeField] private RectTransform canvas;
        
        private PopupManager _popupManager;
        private LabelController _labelController;
        private GameManager _gameManager;
        private SceneSwitcher _sceneSwitcher;

        [GameInject]
        public void Construct(PopupManager popupManager, LabelController labelController,
            GameManager gameManager, SceneSwitcher sceneSwitcher)
        {
            _popupManager = popupManager;
            _labelController = labelController;
            _gameManager = gameManager;
            _sceneSwitcher = sceneSwitcher; 
        }
        
        [GameInit]
        public void Init()
        {
            menuButton.onClick.AddListener(HandleMenuButtonClicked);
        }
        
        [GameFinish]
        public void Finish()
        {
            menuButton.onClick.RemoveAllListeners();
            _popupManager.Clean();
        }
        
        private void HandleMenuButtonClicked()
        {
            _gameManager.PauseGame();
            var builder = new PopupBuilder(_popupManager)
            {
                VerticalScroll = false,
                Fit = true,
                ActivateBackButton = true
            };

            builder.AddBackAction(UniTask.UnityAction(async () =>
            {
                await UniTask.Yield();
                _gameManager.ResumeGame();
            }));
            builder.AddLabel(_labelController, "menu", 60, Color.white)
                .AddButton(_labelController, "restart", 40, new Color32(157, 88, 255, 255), Restart)
                .AddButton(_labelController, "back", 40, new Color32(251, 39, 90, 255), Back)
                .AddButton(_labelController, "continue", 40, new Color32(83, 253, 121, 255), builder.GetBackButtonInvocation);
            
            var popup = builder.Build();
            popup.transform.SetParent(canvas, false);
            popup.gameObject.SetActive(true);
        }

        private void Back()
        {
            _popupManager.Clean();
            _gameManager.Secede();
            _sceneSwitcher.LoadSceneAsync("PacksScene").Forget();
        }

        private async void Restart()
        {
            _gameManager.FinishGame();
            _gameManager.InitGame();
            await UniTask.Yield();
            _gameManager.StartGame();
        }
    }
}