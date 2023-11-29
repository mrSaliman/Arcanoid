using App.Scripts.Configs;
using App.Scripts.Libs.NodeArchitecture;
using App.Scripts.Scenes.AllScenes.ProjectContext;
using App.Scripts.Scenes.AllScenes.ProjectContext.Energy;
using App.Scripts.Scenes.AllScenes.ProjectContext.Packs;
using App.Scripts.Scenes.AllScenes.ProjectContext.Pop_Up;
using App.Scripts.Scenes.AllScenes.UI;
using App.Scripts.Scenes.GameScene.Game;
using App.Scripts.Scenes.PacksScene.Packs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.PacksScene.UI
{
    public class PacksUIController : MonoBehaviour, IContextUpdate
    {
        [SerializeField] private RectTransform packsContainer;
        [SerializeField] private PackButton packButtonPrefab;
        [SerializeField] private Button backButton;
        [SerializeField] private LevelPacks packs;
        [SerializeField] private EnergyBar energyBar;
        [SerializeField] private RectTransform canvas;

        private LabelController _labelController;
        private PopupManager _popupManager;
        private SceneSwitcher _sceneSwitcher;
        private PacksSceneManager _packsSceneManager;
        private PacksController _packsController;
        private EnergyController _energyController;

        [GameInject]
        public void Construct(LabelController labelController, PopupManager popupManager, SceneSwitcher sceneSwitcher,
            PacksSceneManager packsSceneManager, PacksController packsController, EnergyController energyController)
        {
            _labelController = labelController;
            _sceneSwitcher = sceneSwitcher;
            _popupManager = popupManager;
            _packsSceneManager = packsSceneManager;
            _packsController = packsController;
            _energyController = energyController;
        }

        [GameInit]
        public void Init()
        {
            energyBar.maxEnergy = _energyController.Settings.MaxEnergy;
            backButton.onClick.AddListener(UniTask.UnityAction(HandleBackButtonClicked));
            ShowPacks();
        }
        
        private async UniTaskVoid HandleBackButtonClicked()
        {
            _popupManager.Clean();
            _packsSceneManager.Secede();
            await _sceneSwitcher.LoadSceneAsync("MainMenu");
        }

        private void ShowPacks()
        {
            for (var i = 0; i < packs.Packs.Count; i++)
            {
                var pack = packs.Packs[i];
                var packButton = Instantiate(packButtonPrefab, packsContainer);
                packButton.setSprite(pack.galaxyPicture);
                packButton.setColor(pack.buttonColor);
                var packNumber = i;
                packButton.onClick.AddListener(() =>
                {
                    if (!_energyController.UsePlayEnergy())
                    {
                        _popupManager.CreateLowEnergyPopup(canvas, _labelController);
                        return;
                    }
                    _popupManager.Clean();
                    _packsSceneManager.Secede();
                    _packsController.StartLevel(packNumber);
                }); 
                if (!_packsController.PackResults.packs[i].discovered) packButton.Lock();
                _labelController.AddLabel(pack.name, packButton.Label);
                _labelController.AddLabel("not-discovered", packButton.LockedLabel);
                packButton.PackProgress.text = $"{_packsController.PackResults.packs[i].progress}/{pack.levelCount}";
            }
        }

        public void OnUpdate()
        {
            _energyController.UpdateInfo();
            energyBar.SetValues(_energyController.LastInfo);
        }
    }
}