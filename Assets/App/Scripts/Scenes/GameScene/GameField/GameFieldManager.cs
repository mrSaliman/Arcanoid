using App.Scripts.Configs;
using App.Scripts.Libs.ObjectPool;
using App.Scripts.Scenes.AllScenes.ProjectContext.Packs;
using App.Scripts.Scenes.GameScene.Game;
using App.Scripts.Scenes.GameScene.GameField.Ball;
using App.Scripts.Scenes.GameScene.GameField.Block;
using App.Scripts.Scenes.GameScene.GameField.Level;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.GameField
{
    public class GameFieldManager : MonoBehaviour
    {
        public ObjectPool<Block.Block> BlockPool;
        public ObjectPool<BlockView> BlockViewPool;
        public ObjectPool<BallView> BallViewPool;
            
        [SerializeField] public TilesetSettings tilesetSettings;
        [SerializeField] public LevelLoaderSettings levelLoaderSettings;
        [SerializeField] public GameFieldSettings gameFieldSettings;
        [SerializeField] public BallsSettings ballsSettings;

        [SerializeField] private EdgeCollider2D edgeCollider;
        [SerializeField] private BoxCollider2D deathZone;

        public Transform blockContainer;
        public Transform ballContainer;

        private LevelLoader _levelLoader;
        private GameFieldInfoProvider _gameFieldInfoProvider;
        private LevelView _levelView;
        private BallsController _ballsController;
        private HealthController _healthController;
        private PacksController _packsController;

        public Level.Level CurrentLevel { get; private set; }

        [GameInject]
        public void Construct(LevelLoader levelLoader, GameFieldInfoProvider gameFieldInfoProvider, LevelView levelView,
            BallsController ballsController, HealthController healthController, PacksController packsController)
        {
            _levelLoader = levelLoader;
            _gameFieldInfoProvider = gameFieldInfoProvider;
            _levelView = levelView;
            _ballsController = ballsController;
            _healthController = healthController;
            _packsController = packsController;
        }

        [GameInit]
        public void Init()
        {
            SetupWalls();
            LoadCurrentLevel();
            _levelView.BuildLevelView(CurrentLevel);
        }

        [GameStart]
        public void StartGame()
        {
            _ballsController.Speed = ballsSettings.BallSpeed.x;
        }
        
        [GameFinish]
        public void Finish()
        {
            foreach (var block in CurrentLevel.Blocks)
            {
                BlockPool.Return(block);
            }
        }

        private void SetupWalls()
        {
            var cameraRect = _gameFieldInfoProvider.GameFieldRect;
            edgeCollider.points = new[]
            {
                new Vector2(cameraRect.xMin, cameraRect.yMin),
                new Vector2(cameraRect.xMin, cameraRect.yMax),
                new Vector2(cameraRect.xMax, cameraRect.yMax),
                new Vector2(cameraRect.xMax, cameraRect.yMin),
            };
            edgeCollider.enabled = true;

            var deathZoneSize = deathZone.size;
            deathZone.offset = new Vector2(0, cameraRect.yMin - deathZoneSize.y / 2);
            deathZoneSize.x = cameraRect.size.x;
            deathZone.size = deathZoneSize;
            deathZone.enabled = true;
        }

        private void LoadCurrentLevel()
        {
            CurrentLevel = _levelLoader.LoadLevel(_packsController.levelToRun);
        }

        public void RemoveBlock(BlockView blockView)
        {
            _levelView.RemoveBlock(blockView);
            BlockPool.Return(CurrentLevel.GetBlock(blockView.gridPosition.x, blockView.gridPosition.y));
            CurrentLevel.RemoveBlock(blockView.gridPosition.x, blockView.gridPosition.y);
        }

        [Button]
        private void DealDamage(int damage, BlockView tile)
        {
            CurrentLevel.GetBlock(tile.gridPosition.x, tile.gridPosition.y)?.TakeDamage(damage);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out BallView ball))
            {
                _ballsController.DeleteBall(ball);
                _healthController.DealBallDamage(1);
            }
        }
    }
}