using App.Scripts.AllScenes.ProjectContext;
using App.Scripts.Configs;
using App.Scripts.GameScene.Game;
using App.Scripts.GameScene.GameField.Model;
using App.Scripts.GameScene.GameField.View;
using UnityEngine;

namespace App.Scripts.GameScene.GameField
{
    public class GameFieldManager : MonoBehaviour
    {
        [SerializeField] public TilesetSettings tilesetSettings;
        [SerializeField] public LevelLoaderSettings levelLoaderSettings;
        [SerializeField] public GameFieldSettings gameFieldSettings;

        [SerializeField] private EdgeCollider2D edgeCollider;

        private LevelLoader _levelLoader;
        private ProjectContext _projectContext;
        private CameraInfoProvider _cameraInfoProvider;
        private LevelView _levelView;

        private Level _currentLevel;

        [GameInject]
        public void Construct(LevelLoader levelLoader, ProjectContext projectContext, CameraInfoProvider cameraInfoProvider, LevelView levelView)
        {
            _levelLoader = levelLoader;
            _projectContext = projectContext;
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
            _currentLevel = _levelLoader.LoadLevel(_projectContext.currentLevel);
        }
    }
}