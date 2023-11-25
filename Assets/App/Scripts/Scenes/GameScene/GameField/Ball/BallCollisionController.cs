using App.Scripts.Scenes.GameScene.Game;
using App.Scripts.Scenes.GameScene.GameField.Block;
using App.Scripts.Scenes.GameScene.GameField.Platform;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.GameField.Ball
{
    public class BallCollisionController
    {
        private BallsController _ballsController;
        private GameFieldManager _gameFieldManager;

        private float _minAngle;

        [GameInject]
        public void Construct(BallsController ballsController, GameFieldManager manager)
        {
            _ballsController = ballsController;
            _gameFieldManager = manager;
            _minAngle = _gameFieldManager.ballsSettings.MinReflectionAngle;
        }
        
        public void OnCollisionEnter(BallView ball, Collision2D other)
        {
            if (!other.gameObject.TryGetComponent<PlatformView>(out _)) CalibrateDirection(ball, other);
            
            if (other.gameObject.TryGetComponent(out BlockView blockView))
            {
                var block = _gameFieldManager.CurrentLevel.GetBlock(blockView.gridPosition.x, blockView.gridPosition.y); 
                block?.TakeDamage(1);
            }
            
            CalibrateSpeed(ball);
        }
        
        public void OnCollisionExit(BallView ball, Collision2D other)
        {
            CalibrateSpeed(ball);
        }

        private void CalibrateDirection(BallView ball, Collision2D other)
        {
            var currentDirection = ball.MainRigidbody.velocity.normalized;
            var normal = other.GetContact(0).normal;
            var angle = Vector2.Angle(currentDirection, normal);
            
            if (!(angle < _minAngle)) return;
            var sign = Mathf.Sign(Vector3.Cross(normal, currentDirection).z);
            
            var targetAngle = sign * _minAngle;
            Vector2 newDirection = Quaternion.Euler(0, 0, targetAngle) * normal;
                
            ball.MainRigidbody.velocity = newDirection;
        }
        
        private void CalibrateSpeed(BallView ball)
        {
            ball.SetSpeed(_ballsController.Speed);
        }
    }
}