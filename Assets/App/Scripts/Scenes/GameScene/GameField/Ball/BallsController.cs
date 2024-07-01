using System.Collections.Generic;
using App.Scripts.Configs;
using App.Scripts.Libs.NodeArchitecture;
using App.Scripts.Libs.ObjectPool;
using App.Scripts.Scenes.AllScenes.ProjectContext.Packs;
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
        private GameManager _gameManager;
        private HealthController _healthController;

        public float Speed;

        public int Count => _attachedBalls.Count + _balls.Count;

        [GameInject]
        public void Construct(GameFieldManager manager, PlatformView platformView, MouseInput mouseInput,
            BallCollisionController ballCollisionController, GameManager gameManager, HealthController healthController)
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
            _gameManager = gameManager;
            _healthController = healthController;
        }

        [GameStart]
        public void StartGame()
        {
            CreateGluedBall();
        }
        
        [GamePause]
        public void Pause()
        {
            foreach (var ball in _balls)
            {
                ball.SetSimulated(false);
            }
        }

        [GameResume]
        public void Resume()
        {
            foreach (var ball in _balls)
            {
                ball.SetSimulated(true);
            }
        }
        
        [GameFinish]
        public void Finish()
        {
            foreach (var ball in _attachedBalls)
            {
                _ballViewPool.Return(ball);
            }
            _attachedBalls.Clear();

            foreach (var ball in _balls)
            {
                _ballViewPool.Return(ball);
            }
            _balls.Clear();
        }

        public void CalibrateSpeed()
        {
            foreach (var ball in _balls)
            {
                ball.SetSpeed(Speed);
            }
        }

        public void HandleBallDied()
        {
            if (Count != 0) return;
            if (_healthController.DealDamage(1) == 0) _gameManager.EndGame(LevelResult.Lose);
            CreateGluedBall();
        }

        public BallView CreateBall()
        {
            var ball = _ballViewPool.Get();
            ball.OnCollisionEnterEvent += _ballCollisionController.OnCollisionEnter;
            ball.OnCollisionExitEvent += _ballCollisionController.OnCollisionExit;
            _balls.Add(ball);
            return ball;
        }

        public void CreateGluedBall()
        {
            var ball = CreateBall();
            AttachBall(ball);
            ball.Show();
        }

        public void DeleteBall(BallView ball)
        {
            _balls.Remove(ball);
            _ballViewPool.Return(ball);
        }

        public void AttachBall(BallView ball)
        {
            _attachedBalls.Add(ball);
            _balls.Remove(ball);
            var position = GetPlatformPosition(ball);
            ball.SetPosition(position);
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
            _balls.AddRange(_attachedBalls);
            _attachedBalls.Clear();
        }

        public void OnUpdate()
        {
            if (_gameManager.GameState != GameState.Playing) return;
            if (_mouse.LeftButton == MouseInput.ButtonState.Up) ReleaseBalls();
            
            foreach (var ball in _attachedBalls)
            {
                ball.SetPosition(GetPlatformPosition(ball));
            }
        }
    }
}