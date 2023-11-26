using App.Scripts.Configs;
using App.Scripts.Scenes.AllScenes.ProjectContext;
using App.Scripts.Scenes.AllScenes.ProjectContext.Pop_Up;
using App.Scripts.Scenes.AllScenes.UI;
using App.Scripts.Scenes.GameScene.Game;
using App.Scripts.Scenes.PacksScene.Packs;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.PacksScene.UI
{
    public class PacksUIController : MonoBehaviour
    {
        [SerializeField] private RectTransform packsContainer;
        [SerializeField] private PackButton packButtonPrefab;
        [SerializeField] private Button backButton;
        [SerializeField] private LevelPacks packs;

        private LabelController _labelController;
        private PopupManager _popupManager;
        private SceneSwitcher _sceneSwitcher;
        private PacksManager _packsManager;

        [GameInject]
        public void Construct(LabelController labelController, PopupManager popupManager, SceneSwitcher sceneSwitcher,
            PacksManager packsManager)
        {
            _labelController = labelController;
            _sceneSwitcher = sceneSwitcher;
            _popupManager = popupManager;
            _packsManager = packsManager;
        }

        [GameInit]
        public void Init()
        {
            backButton.onClick.AddListener(HandleBackButtonClicked);
            ShowPacks();
        }
        
        private async void HandleBackButtonClicked()
        {
            _popupManager.Clean();
            _packsManager.Secede();
            await _sceneSwitcher.LoadSceneAsync("MainMenu");
        }

        private void ShowPacks()
        {
            foreach (var pack in packs.Packs)
            {
                var packButton = Instantiate(packButtonPrefab, packsContainer);
                packButton.sprite = pack.galaxyPicture;
                packButton.color = pack.buttonColor;
                packButton.onClick.AddListener(() => { }); //TODO add listener
                _labelController.AddLabel(pack.name, packButton.Label);
            }
        }
    }
}