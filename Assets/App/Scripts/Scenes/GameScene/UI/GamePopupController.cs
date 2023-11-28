using App.Scripts.Scenes.AllScenes.ProjectContext;
using App.Scripts.Scenes.AllScenes.ProjectContext.Packs;
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
        private PacksController _packsController;

        private UnityAction _skipInvocation;

        [GameInject]
        public void Construct(PopupManager popupManager, LabelController labelController,
            GameManager gameManager, SceneSwitcher sceneSwitcher, PacksController packsController)
        {
            _popupManager = popupManager;
            _labelController = labelController;
            _gameManager = gameManager;
            _sceneSwitcher = sceneSwitcher;
            _packsController = packsController;
        }
        
        [GameInit]
        public void Init()
        {
            menuButton.onClick.AddListener(HandleMenuButtonClicked);
            _gameManager.OnGameWin += GameWin; 
            _gameManager.OnGameLose += GameLose;
        }
        
        [GameFinish]
        public void Finish()
        {
            _gameManager.OnGameLose -= GameLose;
            _gameManager.OnGameWin -= GameWin;
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
                .AddButton(_labelController, "continue", 40, new Color32(83, 253, 121, 255), builder.GetBackButtonInvocation)
                .AddButton(_labelController, "skip", 40, new Color32(0, 0, 0, 120), SkipButtonClicked);
            
            var popup = builder.Build();
            _skipInvocation = () => _popupManager.Return(popup);
            popup.transform.SetParent(canvas, false);
            popup.gameObject.SetActive(true);
        }
        
        private void SkipButtonClicked()
        {
            _skipInvocation?.Invoke();
            Skip();
        }

        private void GameWin()
        {
            _gameManager.PauseGame();
            var builder = new PopupBuilder(_popupManager)
            {
                VerticalScroll = false,
                Fit = true,
                ActivateBackButton = false
            };
            builder.AddLabel(_labelController, "win", 60, Color.green)
                .AddButton(_labelController, "next", 40, new Color32(157, 88, 255, 255), Next)
                .AddButton(_labelController, "back", 40, new Color32(251, 39, 90, 255), Back);
            
            var popup = builder.Build();
            popup.transform.SetParent(canvas, false);
            popup.gameObject.SetActive(true);
        }

        private void GameLose()
        {
            _gameManager.PauseGame();
            var builder = new PopupBuilder(_popupManager)
            {
                VerticalScroll = false,
                Fit = true,
                ActivateBackButton = false
            };
            builder.AddLabel(_labelController, "lose", 60, Color.red)
                .AddButton(_labelController, "restart", 40, new Color32(157, 88, 255, 255), Restart)
                .AddButton(_labelController, "back", 40, new Color32(251, 39, 90, 255), Back);
            
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

        private void Skip()
        {
            _gameManager.ResumeGame();
            _gameManager.SkipLevel();
        }

        private void Next()
        {
            if (_packsController.NextLevel()) Restart();
            else Back();
        }
    }
}