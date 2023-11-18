using System;
using System.Collections.Generic;
using App.Scripts.Configs;
using App.Scripts.GameScene.Game;
using App.Scripts.GameScene.GameField.Model;
using App.Scripts.Libs.ObjectPool;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Scripts.GameScene.GameField.View
{
    public class LevelView
    {
        private List<BlockView> _blocks = new();
        private ObjectPool<BlockView> _blockViewPool;

        private GameFieldSettings _gameFieldSettings;
        private LevelLoaderSettings _levelLoaderSettings;

        private LevelLoader _levelLoader;
        private GameFieldManager _gameFieldManager;
        private BlockBehaviourHandler _blockBehaviourHandler;
        private Rect _cameraRect;

        private float _blockWidth, _blockHeight, _expandCoefficient;

        [GameInject]
        public void Construct(GameFieldManager manager, LevelLoader levelLoader, CameraInfoProvider cameraInfoProvider,
            BlockBehaviourHandler blockBehaviourHandler)
        {
            _blockBehaviourHandler = blockBehaviourHandler;
            _gameFieldManager = manager;
            _gameFieldSettings = manager.gameFieldSettings;
            _levelLoaderSettings = manager.levelLoaderSettings;
            _cameraRect = cameraInfoProvider.CameraRect;
            _levelLoader = levelLoader;
            manager.BlockViewPool = new ObjectPool<BlockView>(
                () => Object.Instantiate(_gameFieldSettings.BlockViewPrefab, _gameFieldManager.blockContainer),
                _levelLoaderSettings.BlockPoolSize);
            _blockViewPool = manager.BlockViewPool;
        }
        
        public void BuildLevelView(Level level)
        {
            CalculateLevelSizes(level);
            for (var y = 0; y < level.Height; y++)
            {
                for (var x = 0; x < level.Width; x++)
                {
                    var block = level.GetBlock(x, y);
                    if (block is null) continue;
                    var blockView = _blockViewPool.Get();
                    blockView.SetSprite(_levelLoader.Tileset[level.GetTag(x, y)].sprite);
                    blockView.gridPosition = new Vector2Int(x, y);
                    SetBlockPositionAndScale(blockView);
                    
                    Action onHealthDepletedHandler = () => HandleBlockHealthDepleted(blockView);
                    block.OnHealthDepleted += onHealthDepletedHandler;
                    blockView.OnHealthDepletedHandler = onHealthDepletedHandler;
                    _blocks.Add(blockView);
                }
            }
        }

        private void CalculateLevelSizes(Level level)
        {
            _blockWidth = (_cameraRect.width - (2 * _gameFieldSettings.HorizontalIndentation +
                                                _gameFieldSettings.BetweenBlockIndentation * (level.Width - 1))) /
                          level.Width;
            _expandCoefficient = _blockWidth / _gameFieldSettings.DefaultBlockSize.x;
            _blockHeight = _expandCoefficient * _gameFieldSettings.DefaultBlockSize.y;
        }

        private void SetBlockPositionAndScale(BlockView block)
        {
            var transform = block.transform;
            transform.position = new Vector3(
                _blockWidth / 2 + (_blockWidth + _gameFieldSettings.BetweenBlockIndentation) * block.gridPosition.x +
                _gameFieldSettings.HorizontalIndentation + _cameraRect.xMin,
                - _blockHeight / 2 - (_blockHeight + _gameFieldSettings.BetweenBlockIndentation) * block.gridPosition.y -
                _gameFieldSettings.TopIndentationPercent * _cameraRect.height + _cameraRect.yMax);
            transform.localScale = Vector3.one * _expandCoefficient;
        }
        
        private void HandleBlockHealthDepleted(BlockView blockView)
        {
            var block = _gameFieldManager.CurrentLevel.GetBlock(blockView.gridPosition.x, blockView.gridPosition.y);
            block.OnHealthDepleted -= blockView.OnHealthDepletedHandler;
            _blockBehaviourHandler.HandleBlockDeath(blockView, block);
        }

        public void RemoveBlock(BlockView blockView)
        {
            _blocks.Remove(blockView);
            _blockViewPool.Return(blockView);
        }
    }
}