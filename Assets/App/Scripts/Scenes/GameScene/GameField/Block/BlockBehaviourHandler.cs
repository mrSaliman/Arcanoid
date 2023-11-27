using System;
using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Game;
using App.Scripts.Scenes.GameScene.GameField.Ball;
using App.Scripts.Scenes.GameScene.GameField.Level;
using UnityEngine;
using Random = UnityEngine.Random;

namespace App.Scripts.Scenes.GameScene.GameField.Block
{
    public class BlockBehaviourHandler
    {
        private LevelView _levelView;
        private GameFieldManager _gameFieldManager;
        private BallsController _ballsController;

        private List<Sprite> _crackList;

        private float _minSpeed, _speedAlpha; 
        
        [GameInject]
        public void Construct(LevelView levelView, GameFieldManager manager, BallsController ballsController)
        {
            _levelView = levelView;
            _gameFieldManager = manager;
            _ballsController = ballsController;
            _minSpeed = _gameFieldManager.ballsSettings.BallSpeed.x;
            _speedAlpha = _gameFieldManager.ballsSettings.BallSpeed.y - _minSpeed;
            _crackList = _gameFieldManager.gameFieldSettings.Cracks;
        }

        public void HandleBlockHit(BlockView blockView, int damage)
        {
            var block = _gameFieldManager.CurrentLevel.GetBlock(blockView.gridPosition.x, blockView.gridPosition.y); 
            if (block != null && block.TakeDamage(damage)) SwapCrack(blockView);
        }

        private void SwapCrack(BlockView blockView)
        {
            blockView.SetCrack(GetRandomSpriteExcluding(blockView.Crack));
        }

        private Sprite GetRandomSpriteExcluding(Sprite excludedSprite)
        {
            var selectedSprite = _crackList[Random.Range(0, _crackList.Count)];
            
            while (selectedSprite == excludedSprite)
            {
                selectedSprite = _crackList[Random.Range(0, _crackList.Count)];
            }

            return selectedSprite;
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
            _ballsController.Speed = _levelView.Progress * _speedAlpha + _minSpeed;
        }
    }
}