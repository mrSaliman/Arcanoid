using System;
using System.Collections.Generic;
using App.Scripts.Configs;
using App.Scripts.Libs.ObjectPool;
using App.Scripts.Scenes.GameScene.Game;
using App.Scripts.Scenes.GameScene.GameField.Ball;
using App.Scripts.Scenes.GameScene.GameField.Boosts;
using App.Scripts.Scenes.GameScene.GameField.Level;
using App.Scripts.Scenes.GameScene.GameField.Platform;
using Cysharp.Threading.Tasks;
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
        private PlatformView _platformView;

        [SerializeField] private Boost boostPrefab;
        [SerializeField] private BoostsSettings settings;
        [SerializeField] private Transform boostContainer;

        private ObjectPool<Boost> _boostPool;
        private Dictionary<BlockType, Action> _boostActions = new();
        private List<Boost> _boosts = new();
        

        private List<Sprite> _crackList;

        private float _minSpeed, _speedAlpha, _speedModifier = 1; 
        
        [GameInject]
        public void Construct(LevelView levelView, GameFieldManager manager, BallsController ballsController,
            BombExplosionController bombExplosionController, HealthController healthController,
            PlatformView platformView)
        {
            _platformView = platformView;
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
            _speedModifier = 1;
            _boostActions[BlockType.Death] = ActivateDeath;
            _boostActions[BlockType.Heal] = ActivateHeal;
            _boostActions[BlockType.BigPlatform] = ActivateBigPlatform;
            _boostActions[BlockType.FireBall] = ActivateFireBall;
            _boostActions[BlockType.SlowBall] = UniTask.Action(ActivateSlowBall);
            _boostActions[BlockType.SlowPlatform] = UniTask.Action(ActivateSlowPlatform);
            _boostActions[BlockType.SmallPlatform] = ActivateSmallPlatform;
            _boostActions[BlockType.SpeedUpBall] = UniTask.Action(ActivateSpeedUpBall);
            _boostActions[BlockType.SpeedUpPlatform] = UniTask.Action(ActivateSpeedUpPlatform);
        }

        [GamePause]
        public void Pause()
        {
            foreach (var boost in _boosts)
            {
                boost.rb.simulated = false;
            }
        }

        [GameResume]
        public void Resume()
        {
            foreach (var boost in _boosts)
            {
                boost.rb.simulated = true;
            }
        }

        [GameFinish]
        public void Finish()
        {
            foreach (var boost in _boosts)
            {
                _boostPool.Return(boost);
            }
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
                    var ball = _ballsController.CreateBall();
                    ball.SetPosition(blockView.transform.position);
                    ball.Show();
                    ball.SetVelocity(Random.insideUnitCircle);
                    ball.SetSpeed(_ballsController.Speed);
                    ball.SetSimulated(true);
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
                    _boosts.Add(boost);
                    boost.spriteRenderer.sprite = settings.Boosts[block.blockType].sprite;
                    boost.OnBoostApplied = _boostActions[block.blockType];
                    boost.OnBoostApplied += () =>
                    {
                        _boosts.Remove(boost);
                        _boostPool.Return(boost);
                    };
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
            _ballsController.Speed = (_levelView.Progress * _speedAlpha + _minSpeed) * _speedModifier;
        }

        private void ActivateBigPlatform()
        {
            Debug.Log("Touch");
        }
        
        private async UniTaskVoid ActivateSpeedUpBall()
        {
            var lastSpeed = _ballsController.Speed;
            _speedModifier = settings.Boosts[BlockType.SpeedUpBall].impact;
            _ballsController.Speed *= settings.Boosts[BlockType.SpeedUpBall].impact;
            _ballsController.CalibrateSpeed();

            await UniTask.WaitForSeconds(settings.Boosts[BlockType.SpeedUpBall].duration);
            _ballsController.Speed = lastSpeed;
            _speedModifier = 1;
            _ballsController.CalibrateSpeed();
        }
        private async UniTaskVoid ActivateSpeedUpPlatform()
        {
            _platformView.speedMultiplier = settings.Boosts[BlockType.SpeedUpPlatform].impact;
            await UniTask.WaitForSeconds(settings.Boosts[BlockType.SpeedUpPlatform].duration);
            _platformView.speedMultiplier = 1;
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
        private async UniTaskVoid ActivateSlowBall()
        {
            var lastSpeed = _ballsController.Speed;
            _ballsController.Speed *= settings.Boosts[BlockType.SlowBall].impact;
            _speedModifier = settings.Boosts[BlockType.SlowBall].impact;
            _ballsController.CalibrateSpeed();

            await UniTask.WaitForSeconds(settings.Boosts[BlockType.SlowBall].duration);
            _ballsController.Speed = lastSpeed;
            _speedModifier = 1;
            _ballsController.CalibrateSpeed();
        }
        private async UniTaskVoid ActivateSlowPlatform()
        {
            _platformView.speedMultiplier = settings.Boosts[BlockType.SlowPlatform].impact;
            await UniTask.WaitForSeconds(settings.Boosts[BlockType.SlowPlatform].duration);
            _platformView.speedMultiplier = 1;
        }
        private void ActivateSmallPlatform()
        {
            Debug.Log("Touch");
        }
    }
}