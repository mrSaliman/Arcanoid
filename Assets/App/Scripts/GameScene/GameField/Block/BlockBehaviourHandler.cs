using System;
using App.Scripts.GameScene.Game;
using App.Scripts.GameScene.GameField.Ball;
using App.Scripts.GameScene.GameField.Level;
using UnityEngine;

namespace App.Scripts.GameScene.GameField.Block
{
    public class BlockBehaviourHandler
    {
        private LevelView _levelView;
        private GameFieldManager _gameFieldManager;
        private BallsController _ballsController;

        private float _minSpeed, _speedAlpha; 
        
        [GameInject]
        public void Construct(LevelView levelView, GameFieldManager manager, BallsController ballsController)
        {
            _levelView = levelView;
            _gameFieldManager = manager;
            _ballsController = ballsController;
            _minSpeed = _gameFieldManager.ballsSettings.BallSpeed.x;
            _speedAlpha = _gameFieldManager.ballsSettings.BallSpeed.y - _minSpeed;
        }
        
        public void HandleBlockDeath(BlockView blockView, Block block)
        {
            switch (block.blockType)
            {
                case BlockType.Simple:
                    break;
                case BlockType.Iron:
                    break;
                case BlockType.TNT:
                    break;
                case BlockType.VerticalTNT:
                    break;
                case BlockType.HorizontalTNT:
                    break;
                case BlockType.ColorTNT:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Delete(blockView);
        }

        private void Delete(BlockView blockView)
        {
            _gameFieldManager.RemoveBlock(blockView);
            _ballsController.Speed = _levelView.Difficulity * _speedAlpha + _minSpeed;
        }
    }
}