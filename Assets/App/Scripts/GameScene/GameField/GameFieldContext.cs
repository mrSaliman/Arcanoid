using App.Scripts.GameScene.Game;
using App.Scripts.GameScene.GameField.Ball;
using App.Scripts.GameScene.GameField.Block;
using App.Scripts.GameScene.GameField.Level;
using App.Scripts.GameScene.GameField.Platform;
using App.Scripts.Libs.NodeArchitecture;
using UnityEngine;

namespace App.Scripts.GameScene.GameField
{
    public class GameFieldContext : ContextNode
    {
        [SerializeField] private GameFieldManager gameFieldManager;
        [SerializeField] private PlatformView platformView;
        private LevelLoader _levelLoader = new();
        private LevelView _levelView = new();
        private BlockBehaviourHandler _blockBehaviourHandler = new();
        private PlatformMover _platformMover = new();
        private BallsController _ballsController = new();
        private BallCollisionController _ballCollisionController = new();
        
        protected override void OnConstruct()
        {
            RegisterInstance(gameFieldManager);
            RegisterInstance(_levelLoader);
            RegisterInstance(_levelView);
            RegisterInstance(_blockBehaviourHandler);
            RegisterInstance(platformView);
            RegisterInstance(_platformMover);
            RegisterInstance(_ballsController);
            RegisterInstance(_ballCollisionController);
        }
    }
}