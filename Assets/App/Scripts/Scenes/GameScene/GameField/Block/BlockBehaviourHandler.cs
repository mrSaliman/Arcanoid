using System;
using System.Collections.Generic;
using App.Scripts.Configs;
using App.Scripts.Libs.ObjectPool;
using App.Scripts.Scenes.GameScene.Game;
using App.Scripts.Scenes.GameScene.GameField.Ball;
using App.Scripts.Scenes.GameScene.GameField.Boosts;
using App.Scripts.Scenes.GameScene.GameField.Level;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace App.Scripts.Scenes.GameScene.GameField.Block
{
    [Serializable]
    public class BlockBehaviourHandler
    {
        private LevelView _levelView;
        private GameFieldManager _gameFieldManager;
        private BallsController _ballsController;
        private BombExplosionController _bombExplosionController;
        private HealthController _healthController;

        [SerializeField] private Boost boostPrefab;
        [SerializeField] private BoostsSettings settings;
        [SerializeField] private Transform boostContainer;

        private ObjectPool<Boost> _boostPool;
        private Dictionary<BlockType, Action> _boostActions = new();
        

        private List<Sprite> _crackList;

        private float _minSpeed, _speedAlpha; 
        
        [GameInject]
        public void Construct(LevelView levelView, GameFieldManager manager, BallsController ballsController,
            BombExplosionController bombExplosionController, HealthController healthController)
        {
            _healthController = healthController;
            _bombExplosionController = bombExplosionController;
            _levelView = levelView;
            _gameFieldManager = manager;
            _ballsController = ballsController;
            _minSpeed = _gameFieldManager.ballsSettings.BallSpeed.x;
            _speedAlpha = _gameFieldManager.ballsSettings.BallSpeed.y - _minSpeed;
            _crackList = _gameFieldManager.gameFieldSettings.Cracks;

            _boostPool = new ObjectPool<Boost>(() => Object.Instantiate(boostPrefab, boostContainer), 10);
        }

        [GameInit]
        public void Init()
        {
            _boostActions[BlockType.Death] = ActivateDeath;
            _boostActions[BlockType.Heal] = ActivateHeal;
            _boostActions[BlockType.BigPlatform] = ActivateBigPlatform;
            _boostActions[BlockType.FireBall] = ActivateFireBall;
            _boostActions[BlockType.SlowBall] = ActivateSlowBall;
            _boostActions[BlockType.SlowPlatform] = ActivateSlowPlatform;
            _boostActions[BlockType.SmallPlatform] = ActivateSmallPlatform;
            _boostActions[BlockType.SpeedUpBall] = ActivateSpeedUpBall;
            _boostActions[BlockType.SpeedUpPlatform] = ActivateSpeedUpPlatform;
        }

        public void HandleBlockHit(BlockView blockView, int damage)
        {
            var block = _gameFieldManager.CurrentLevel.GetBlock(blockView.gridPosition.x, blockView.gridPosition.y); 
            if (block != null && block.TakeDamage(damage)) AddCrack(blockView);
        }

        private void AddCrack(BlockView blockView)
        {
            blockView.AddCrack(GetRandomSpriteExcluding(blockView.LastCrack));
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
                case BlockType.VerticalTNT:
                case BlockType.HorizontalTNT:
                case BlockType.ColorTNT:
                    _bombExplosionController.Explode(blockView, block);
                    break;
                case BlockType.AddBall:
                    break;
                case BlockType.BigPlatform:
                case BlockType.SpeedUpBall:
                case BlockType.SpeedUpPlatform:
                case BlockType.Heal:
                case BlockType.Death:
                case BlockType.FireBall:
                case BlockType.SlowBall:
                case BlockType.SlowPlatform:
                case BlockType.SmallPlatform:
                    var boost = _boostPool.Get();
                    boost.spriteRenderer.sprite = settings.Boosts[block.blockType].sprite;
                    boost.OnBoostApplied = _boostActions[block.blockType];
                    boost.OnBoostApplied += () => _boostPool.Return(boost);
                    boost.transform.position = blockView.transform.position;
                    boost.gameObject.SetActive(true);
                    boost.rb.velocity = new Vector2(0, settings.BoostSpeed);
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

        private void ActivateBigPlatform()
        {
            Debug.Log("Touch");
        }
        
        private void ActivateSpeedUpBall()
        {
            Debug.Log("Touch");
        }
        private void ActivateSpeedUpPlatform()
        {
            Debug.Log("Touch");
        }
        private void ActivateHeal()
        {
            _healthController.DealDamage(-(int)settings.Boosts[BlockType.Heal].impact);
        }
        private void ActivateDeath()
        {
            _healthController.DealDamage((int)settings.Boosts[BlockType.Death].impact);
        }
        private void ActivateFireBall()
        {
            Debug.Log("Touch");
        }
        private void ActivateSlowBall()
        {
            Debug.Log("Touch");
        }
        private void ActivateSlowPlatform()
        {
            Debug.Log("Touch");
        }
        private void ActivateSmallPlatform()
        {
            Debug.Log("Touch");
        }
    }
}