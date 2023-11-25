using System.Collections.Generic;
using App.Scripts.Configs;
using App.Scripts.Libs.NodeArchitecture;
using App.Scripts.Libs.ObjectPool;
using App.Scripts.Scenes.GameScene.Game;
using App.Scripts.Scenes.GameScene.GameField.Platform;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Scripts.Scenes.GameScene.GameField.Ball
{
    public class BallsController : IContextUpdate
    {
        private List<BallView> _attachedBalls = new();
        
        private ObjectPool<BallView> _ballViewPool;
        private List<BallView> _balls = new();

        private BallsSettings _ballsSettings;

        private GameFieldManager _gameFieldManager;
        private PlatformView _platformView;
        private MouseInput _mouse;
        private BallCollisionController _ballCollisionController;

        public float Speed;

        [GameInject]
        public void Construct(GameFieldManager manager, PlatformView platformView, MouseInput mouseInput, BallCollisionController ballCollisionController)
        {
            _gameFieldManager = manager;
            _ballsSettings = _gameFieldManager.ballsSettings;
            _gameFieldManager.BallViewPool = new ObjectPool<BallView>(
                () => Object.Instantiate(_gameFieldManager.gameFieldSettings.BallViewPrefab,
                    _gameFieldManager.ballContainer), _ballsSettings.BallPoolSize);
            _ballViewPool = _gameFieldManager.BallViewPool;
            _platformView = platformView;
            _mouse = mouseInput;
            _ballCollisionController = ballCollisionController;
        }

        public BallView CreateBall()
        {
            var ball = _ballViewPool.Get();
            ball.OnCollisionEnterEvent += _ballCollisionController.OnCollisionEnter;
            ball.OnCollisionExitEvent += _ballCollisionController.OnCollisionExit;
            _balls.Add(ball);
            return ball;
        }

        public void DeleteBall(BallView ball)
        {
            _balls.Remove(ball);
            _ballViewPool.Return(ball);
        }

        public void AttachBall(BallView ball)
        {
            _attachedBalls.Add(ball);
            var position = GetPlatformPosition(ball);
            //ball.SetPosition(position);
            ball.transform.position = position;
            ball.SetSimulated(true);
        }

        private Vector3 GetPlatformPosition(BallView ball)
        {
            return _platformView.GetAttachedBallPosition() + new Vector3(0,
                ball.SpriteRenderer.bounds.size.y / 2);
        }

        private void ReleaseBalls()
        {
            foreach (var ball in _attachedBalls)
            {
                ball.SetVelocity(new Vector2(_platformView.PlatformRigidbody.velocity.x, Speed));
                ball.SetSpeed(Speed);
            }
            _attachedBalls.Clear();
        }

        public void OnUpdate()
        {
            if (_mouse.LeftButton == MouseInput.ButtonState.Up) ReleaseBalls();
            
            foreach (var ball in _attachedBalls)
            {
                ball.SetPosition(GetPlatformPosition(ball));
            }
        }
    }
}