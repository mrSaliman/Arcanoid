using App.Scripts.Libs.NodeArchitecture;
using App.Scripts.Scenes.AllScenes.UI;
using UnityEngine;

namespace App.Scripts.Scenes.PacksScene.UI
{
    public class PacksUIContext : ContextNode
    {
        [SerializeField] private LabelController labelController;
        [SerializeField] private PacksUIController uiController;

        protected override void OnConstruct()
        {
            RegisterInstance(labelController);
            RegisterInstance(uiController);
        }
    }
}