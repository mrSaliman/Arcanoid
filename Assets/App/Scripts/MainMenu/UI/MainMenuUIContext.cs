using App.Scripts.AllScenes.UI;
using App.Scripts.Libs.NodeArchitecture;
using UnityEngine;

namespace App.Scripts.MainMenu.UI
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