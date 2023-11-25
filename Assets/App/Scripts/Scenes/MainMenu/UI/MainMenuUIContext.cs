using App.Scripts.Libs.NodeArchitecture;
using App.Scripts.Scenes.AllScenes.UI;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenu.UI
{
    public class MainMenuUIContext : ContextNode
    {
        [SerializeField] private LabelController labelController;
        [SerializeField] private MainMenuUIController uiController;

        protected override void OnConstruct()
        {
            RegisterInstance(labelController);
            RegisterInstance(uiController);
        }
    }
}