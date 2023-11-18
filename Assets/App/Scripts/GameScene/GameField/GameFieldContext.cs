using App.Scripts.GameScene.Game;
using App.Scripts.GameScene.GameField.Model;
using App.Scripts.GameScene.GameField.View;
using App.Scripts.Libs.NodeArchitecture;
using UnityEngine;

namespace App.Scripts.GameScene.GameField
{
    public class GameFieldContext : ContextNode
    {
        [SerializeField] private GameFieldManager gameFieldManager;
        private LevelLoader _levelLoader = new();
        private LevelView _levelView = new();
        private BlockBehaviourHandler _blockBehaviourHandler = new();
        private PlatformMover _platformMover = new();
        [SerializeField] private PlatformView platformView;
        
        protected override void OnConstruct()
        {
            RegisterInstance(gameFieldManager);
            RegisterInstance(_levelLoader);
            RegisterInstance(_levelView);
            RegisterInstance(_blockBehaviourHandler);
            RegisterInstance(platformView);
            RegisterInstance(_platformMover);
        }
    }
}