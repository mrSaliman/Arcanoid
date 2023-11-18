using App.Scripts.AllScenes.ProjectContext;
using App.Scripts.Configs;
using App.Scripts.GameScene.Game;
using App.Scripts.GameScene.GameField.Model;
using App.Scripts.GameScene.GameField.View;
using App.Scripts.Libs.ObjectPool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.GameScene.GameField
{
    public class GameFieldManager : MonoBehaviour
    {
        public ObjectPool<Block> BlockPool;
        public ObjectPool<BlockView> BlockViewPool;

        [SerializeField] public TilesetSettings tilesetSettings;
        [SerializeField] public LevelLoaderSettings levelLoaderSettings;
        [SerializeField] public GameFieldSettings gameFieldSettings;

        [SerializeField] private EdgeCollider2D edgeCollider;

        public Transform blockContainer;
        public Transform ballContainer;

        private LevelLoader _levelLoader;
        private CameraInfoProvider _cameraInfoProvider;
        private LevelView _levelView;

        private Level _currentLevel;

        public Level CurrentLevel => _currentLevel;

        [GameInject]
        public void Construct(LevelLoader levelLoader, CameraInfoProvider cameraInfoProvider, LevelView levelView)
        {
            _levelLoader = levelLoader;
            _cameraInfoProvider = cameraInfoProvider;
            _levelView = levelView;
        }

        [GameInit]
        public void Init()
        {
            SetupWalls();
            LoadCurrentLevel();
            _levelView.BuildLevelView(_currentLevel);
        }

        private void SetupWalls()
        {
            edgeCollider.enabled = true;
            var cameraRect = _cameraInfoProvider.CameraRect;
            edgeCollider.points = new[]
            {
                new Vector2(cameraRect.xMin, cameraRect.yMin),
                new Vector2(cameraRect.xMin, cameraRect.yMax),
                new Vector2(cameraRect.xMax, cameraRect.yMax),
                new Vector2(cameraRect.xMax, cameraRect.yMin),
                new Vector2(cameraRect.xMin, cameraRect.yMin),
            };
        }

        private void LoadCurrentLevel()
        {
            _currentLevel = _levelLoader.LoadLevel(gameFieldSettings.DebugLevel);
        }

        public void RemoveBlock(BlockView blockView)
        {
            _levelView.RemoveBlock(blockView);
            BlockPool.Return(_currentLevel.GetBlock(blockView.gridPosition.x, blockView.gridPosition.y));
            _currentLevel.RemoveBlock(blockView.gridPosition.x, blockView.gridPosition.y);
        }

        [Button]
        private void DealDamage(int damage, BlockView tile)
        {
            _currentLevel.GetBlock(tile.gridPosition.x, tile.gridPosition.y).TakeDamage(damage);
        }
    }
}