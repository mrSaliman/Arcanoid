using App.Scripts.Libs.NodeArchitecture;
using App.Scripts.Scenes.AllScenes.ProjectContext;
using App.Scripts.Scenes.AllScenes.ProjectContext.Energy;
using App.Scripts.Scenes.AllScenes.ProjectContext.Packs;
using App.Scripts.Scenes.AllScenes.ProjectContext.Pop_Up;
using App.Scripts.Scenes.AllScenes.UI;
using App.Scripts.Scenes.GameScene.Game;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.UI
{
    public class GamePopupController : MonoBehaviour, IContextUpdate
    {
        [SerializeField] private Button menuButton;
        [SerializeField] private RectTransform canvas;
        
        private PopupManager _popupManager;
        private LabelController _labelController;
        private GameManager _gameManager;
        private SceneSwitcher _sceneSwitcher;
        private PacksController _packsController;
        private EnergyController _energyController;
        private LocalizationManager _localizationManager;

        private UnityAction _skipInvocation;
        
        [SerializeField] private EnergyBar energyBarPrefab;
        [SerializeField] private WinGalaxy winGalaxyPrefab;

        private EnergyBar _currentEnergyBar;
        private WinGalaxy _currentWinGalaxy;

        [GameInject]
        public void Construct(PopupManager popupManager, LabelController labelController,
            GameManager gameManager, SceneSwitcher sceneSwitcher, PacksController packsController,
            EnergyController energyController, LocalizationManager localizationManager)
        {
            _energyController = energyController;
            _popupManager = popupManager;
            _labelController = labelController;
            _gameManager = gameManager;
            _sceneSwitcher = sceneSwitcher;
            _packsController = packsController;
            _localizationManager = localizationManager;
        }
        
        [GameInit]
        public void Init()
        {
            if (_currentWinGalaxy == null) _currentWinGalaxy = Instantiate(winGalaxyPrefab, canvas);
            if (_packsController.StartedPack > -1)
            {
                var pack = _packsController.Packs.Packs[_packsController.StartedPack];
                var packResult = _packsController.PackResults.packs[_packsController.StartedPack];

                _currentWinGalaxy.packProgress.text = $"{packResult.nextLevel + 1} / {pack.levelCount}";
                _currentWinGalaxy.packGalaxy.sprite = pack.galaxyPicture;
                _currentWinGalaxy.galaxyName.text = _localizationManager.GetLocalizedString(pack.name);
            }
            _currentWinGalaxy.gameObject.SetActive(false);
            
            
            if (_currentEnergyBar == null) _currentEnergyBar = Instantiate(energyBarPrefab, canvas);
            _currentEnergyBar.maxEnergy = _energyController.Settings.MaxEnergy;
            _currentEnergyBar.gameObject.SetActive(false);
            
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
            
            builder.AddUIElement(_currentEnergyBar.gameObject, DeactivateEnergyBar)
                .AddLabel(_labelController, "menu", 60, Color.white)
                .AddButton(_labelController, "restart", 40, new Color32(157, 88, 255, 255), Restart)
                .AddButton(_labelController, "back", 40, new Color32(251, 39, 90, 255), Back)
                .AddButton(_labelController, "continue", 40, new Color32(83, 253, 121, 255), builder.GetBackButtonInvocation)
                .AddButton(_labelController, "skip", 40, new Color32(0, 0, 0, 120), SkipButtonClicked);
            
            var popup = builder.Build();
            _skipInvocation = () => _popupManager.Return(popup);
            popup.transform.SetParent(canvas, false);
            popup.gameObject.SetActive(true);
            
            _currentEnergyBar.gameObject.SetActive(true);
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
            builder.AddUIElement(_currentEnergyBar.gameObject, DeactivateEnergyBar)
                .AddLabel(_labelController, "win", 60, Color.green)
                .AddUIElement(_currentWinGalaxy.gameObject, DeactivateWinGalaxy)
                .AddButton(_labelController, "next", 40, new Color32(157, 88, 255, 255), Next)
                .AddButton(_labelController, "back", 40, new Color32(251, 39, 90, 255), Back);
            
            var popup = builder.Build();
            popup.transform.SetParent(canvas, false);
            popup.gameObject.SetActive(true);
            
            if (_packsController.StartedPack > -1)
            {
                var pack = _packsController.Packs.Packs[_packsController.StartedPack];
                var packResult = _packsController.PackResults.packs[_packsController.StartedPack];

                if (packResult.nextLevel == 0)
                {
                    if (_packsController.Packs.Packs.Count > _packsController.StartedPack + 1)
                    {
                        pack = _packsController.Packs.Packs[_packsController.StartedPack + 1];
                        packResult = _packsController.PackResults.packs[_packsController.StartedPack + 1];
                        _currentWinGalaxy.packProgress.text = $"{packResult.nextLevel + 1} / {pack.levelCount}";
                    }
                    else
                    {
                        _currentWinGalaxy.packProgress.text = $"{pack.levelCount} / {pack.levelCount}";
                    }
                }
                else
                {
                    _currentWinGalaxy.packProgress.text = $"{packResult.nextLevel + 1} / {pack.levelCount}";
                }
                _currentWinGalaxy.packGalaxy.sprite = pack.galaxyPicture;
                _currentWinGalaxy.galaxyName.text = _localizationManager.GetLocalizedString(pack.name);
            }
            _currentWinGalaxy.star.transform.DOLocalRotate(new Vector3(0, 0, 360), 2f, RotateMode.FastBeyond360)
                .SetLoops(-1).SetRelative(true).SetEase(Ease.Linear);
            _currentWinGalaxy.gameObject.SetActive(true);
            
            _currentEnergyBar.gameObject.SetActive(true);
            _energyController.AddWinEnergy();
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
            builder.AddUIElement(_currentEnergyBar.gameObject, DeactivateEnergyBar)
                .AddLabel(_labelController, "lose", 60, Color.red)
                .AddButton(_labelController, "restart", 40, new Color32(157, 88, 255, 255), Restart)
                .AddButton(_labelController, "back", 40, new Color32(251, 39, 90, 255), Back);
            
            var popup = builder.Build();
            popup.transform.SetParent(canvas, false);
            popup.gameObject.SetActive(true);
            
            _currentEnergyBar.gameObject.SetActive(true);
        }

        private void Back()
        {
            _popupManager.Clean();
            _gameManager.Secede();
            _sceneSwitcher.LoadSceneAsync("PacksScene").Forget();
        }

        private async void Restart()
        {
            if (!_energyController.UsePlayEnergy())
            {
                _popupManager.CreateLowEnergyPopup(canvas, _labelController);
                return;
            }
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

        private void DeactivateEnergyBar()
        {
            _currentEnergyBar.gameObject.SetActive(false);
            _currentEnergyBar.SetValues(new EnergyInfo {Amount = 0});
        }

        private void DeactivateWinGalaxy()
        {
            _currentWinGalaxy.gameObject.SetActive(false);
            _currentWinGalaxy.star.transform.DOKill();
        }

        public void OnUpdate()
        {
            if (!_currentEnergyBar.gameObject.activeSelf) return;
            _energyController.UpdateInfo();
            _currentEnergyBar.SetValues(_energyController.LastInfo);
        }
    }
}