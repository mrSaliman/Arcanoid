using App.Scripts.Libs.NodeArchitecture;
using App.Scripts.Scenes.GameScene.GameField.Ball;
using App.Scripts.Scenes.GameScene.GameField.Block;
using App.Scripts.Scenes.GameScene.GameField.Level;
using App.Scripts.Scenes.GameScene.GameField.Platform;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Scenes.GameScene.GameField
{
    public class GameFieldContext : ContextNode
    {
        [SerializeField] private GameFieldManager gameFieldManager;
        [SerializeField] private PlatformView platformView;
        private LevelLoader _levelLoader = new();
        private LevelView _levelView = new();
        [SerializeField] private BlockBehaviourHandler blockBehaviourHandler;
        private PlatformMover _platformMover = new();
        private BallsController _ballsController = new();
        private BallCollisionController _ballCollisionController = new();
        [SerializeField] private BombExplosionController bombExplosionController;
        
        protected override void OnConstruct()
        {
            RegisterInstance(_levelLoader);
            RegisterInstance(_levelView);
            RegisterInstance(blockBehaviourHandler);
            RegisterInstance(platformView);
            RegisterInstance(_platformMover);
            RegisterInstance(_ballsController);
            RegisterInstance(_ballCollisionController);
            RegisterInstance(bombExplosionController);
            RegisterInstance(gameFieldManager);
        }
    }
}