using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Configs;
using App.Scripts.Libs.DataManager;
using App.Scripts.Libs.ObjectPool;
using App.Scripts.Scenes.AllScenes.ProjectContext.Packs;
using App.Scripts.Scenes.GameScene.Game;
using App.Scripts.Scenes.GameScene.GameField.Block;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Scripts.Scenes.GameScene.GameField.Level
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
        private DataManager _dataManager;
        private GameManager _gameManager;
        private Rect _cameraRect;

        private float _blockWidth, _blockHeight, _expandCoefficient;

        private float _startBlockAmount, _currentCount;
        public float Progress => 1 - _currentCount / _startBlockAmount;

        [GameInject]
        public void Construct(GameFieldManager manager, LevelLoader levelLoader, GameFieldInfoProvider gameFieldInfoProvider,
            BlockBehaviourHandler blockBehaviourHandler, DataManager dataManager, GameManager gameManager)
        {
            _gameManager = gameManager;
            _dataManager = dataManager;
            _blockBehaviourHandler = blockBehaviourHandler;
            _gameFieldManager = manager;
            _gameFieldSettings = manager.gameFieldSettings;
            _levelLoaderSettings = manager.levelLoaderSettings;
            _cameraRect = gameFieldInfoProvider.GameFieldRect;
            _levelLoader = levelLoader;
            manager.BlockViewPool = new ObjectPool<BlockView>(
                () => Object.Instantiate(_gameFieldSettings.BlockViewPrefab, _gameFieldManager.blockContainer),
                _levelLoaderSettings.BlockPoolSize);
            _blockViewPool = manager.BlockViewPool;
        }

        [GameInit]
        public void Init()
        {
            _gameManager.OnSkipLevel += KillAll;
        }

        [GameFinish]
        public void Finish()
        {
            foreach (var blockView in _blocks)
            {
                _blockViewPool.Return(blockView);
            }
            _blocks.Clear();
        }
        
        public void BuildLevelView(Level level)
        {
            _startBlockAmount = 0;
            CalculateLevelSizes(level);
            for (var y = 0; y < level.Height; y++)
            {
                for (var x = 0; x < level.Width; x++)
                {
                    var block = level.GetBlock(x, y);
                    if (block is null) continue;
                    if (block.blockType != BlockType.Iron) _startBlockAmount++;
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

            _currentCount = _startBlockAmount;
        }

        private void CalculateLevelSizes(Level level)
        {
            _blockWidth = (_cameraRect.width - 
                           (2 * _gameFieldSettings.HorizontalIndentation +
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
            var block = _gameFieldManager.CurrentLevel.GetBlock(blockView.gridPosition.x, blockView.gridPosition.y);
            if (block.blockType != BlockType.Iron)
            {
                _currentCount--;
                _dataManager.ModifyData("score", Progress);
                if (_currentCount == 0) _gameManager.EndGame(LevelResult.Win);
            }
            _blocks.Remove(blockView);
            _blockViewPool.Return(blockView);
        }

        private void KillAll()
        {
            var temp = _blocks.Aggregate<BlockView, Action>(null,
                (current, blockView) => current + (() =>
                    _gameFieldManager.CurrentLevel.GetBlock(blockView.gridPosition.x, blockView.gridPosition.y)
                        ?.TakeDamage(999)));

            temp?.Invoke();
        }
    }
}