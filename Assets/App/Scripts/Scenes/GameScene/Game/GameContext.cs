using App.Scripts.Libs.DataManager;
using App.Scripts.Libs.NodeArchitecture;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Scenes.GameScene.Game
{
    public sealed class GameContext : ContextNode
    {
        private DataManager _dataManager = new();
        private MouseInput _mouseInput = new();
        [SerializeField] private GameFieldInfoProvider gameFieldInfoProvider;
        [SerializeField] private HealthController healthController = new();
        
        protected override void OnConstruct()
        {
            RegisterInstance(_mouseInput);
            RegisterInstance(_dataManager);
            RegisterInstance(gameFieldInfoProvider);
            RegisterInstance(healthController);
        }
    }
}