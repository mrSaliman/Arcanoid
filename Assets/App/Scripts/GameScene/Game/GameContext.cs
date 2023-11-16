using App.Scripts.Libs.DataManager;
using App.Scripts.Libs.NodeArchitecture;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.GameScene.Game
{
    public sealed class GameContext : ContextNode
    {
        private DataManager _dataManager = new();
        [SerializeField] private CameraInfoProvider cameraInfoProvider;
        
        protected override void OnConstruct()
        {
            RegisterInstance(_dataManager);
            RegisterInstance(cameraInfoProvider);
        }
    }
}