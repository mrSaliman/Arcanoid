using App.Scripts.Libs.NodeArchitecture;
using App.Scripts.Scenes.AllScenes.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Scenes.GameScene.UI
{
    public class GameUIContext : ContextNode
    {
        [SerializeField] private LabelController labelController;
        [SerializeField] private HealthViewController healthViewController;
        [SerializeField] private GamePopupController gamePopupController;

        protected override void OnConstruct()
        {
            RegisterInstance(labelController);
            RegisterInstance(healthViewController);
            RegisterInstance(gamePopupController);
        }
    }
}