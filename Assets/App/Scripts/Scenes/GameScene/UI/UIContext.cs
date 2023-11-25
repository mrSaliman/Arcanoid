using App.Scripts.Libs.NodeArchitecture;
using App.Scripts.Scenes.AllScenes.UI;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.UI
{
    public class UIContext : ContextNode
    {
        [SerializeField] private LabelController labelController;
        [SerializeField] private HealthViewController healthViewController;

        protected override void OnConstruct()
        {
            RegisterInstance(labelController);
            RegisterInstance(healthViewController);
        }
    }
}