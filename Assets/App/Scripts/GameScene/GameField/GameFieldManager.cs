using App.Scripts.Configs;
using App.Scripts.GameScene.Game;
using App.Scripts.GameScene.GameField.Ball;
using App.Scripts.GameScene.GameField.Block;
using App.Scripts.GameScene.GameField.Level;
using App.Scripts.Libs.ObjectPool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.GameScene.GameField
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
        private CameraInfoProvider _cameraInfoProvider;
        private LevelView _levelView;
        private BallsController _ballsController;

        public Level.Level CurrentLevel { get; private set; }

        [GameInject]
        public void Construct(LevelLoader levelLoader, CameraInfoProvider cameraInfoProvider, LevelView levelView,
            BallsController ballsController)
        {
            _levelLoader = levelLoader;
            _cameraInfoProvider = cameraInfoProvider;
            _levelView = levelView;
            _ballsController = ballsController;
        }

        [GameInit]
        public void Init()
        {
            SetupWalls();
            LoadCurrentLevel();
            _levelView.BuildLevelView(CurrentLevel);
            _ballsController.Speed = ballsSettings.BallSpeed.x;
            CreateGluedBall();
        }

        private void SetupWalls()
        {
            var cameraRect = _cameraInfoProvider.CameraRect;
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
            CurrentLevel = _levelLoader.LoadLevel(gameFieldSettings.DebugLevel);
        }

        public void RemoveBlock(BlockView blockView)
        {
            _levelView.RemoveBlock(blockView);
            BlockPool.Return(CurrentLevel.GetBlock(blockView.gridPosition.x, blockView.gridPosition.y));
            CurrentLevel.RemoveBlock(blockView.gridPosition.x, blockView.gridPosition.y);
        }

        [Button]
        private void CreateGluedBall()
        {
            var ball = _ballsController.CreateBall();
            _ballsController.AttachBall(ball);
            ball.Show();
        }

        [Button]
        private void DealDamage(int damage, BlockView tile)
        {
            CurrentLevel.GetBlock(tile.gridPosition.x, tile.gridPosition.y).TakeDamage(damage);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out BallView ball))
            {
                _ballsController.DeleteBall(ball);
            }
        }
    }
}