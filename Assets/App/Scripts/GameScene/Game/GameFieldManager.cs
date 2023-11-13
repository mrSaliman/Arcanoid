using App.Scripts.Configs;
using App.Scripts.GameScene.Level;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.GameScene.Game
{
    public class GameFieldManager : MonoBehaviour
    {
        [SerializeField] public TilesetSettings tilesetSettings;
        [SerializeField] public LevelLoaderSettings levelLoaderSettings;

        private LevelLoader _levelLoader;

        [GameInject]
        public void Construct(LevelLoader levelLoader)
        {
            _levelLoader = levelLoader;
        }

        [GameInit]
        public void Init()
        {
            _levelLoader.LoadTileset("GameField/Tilesets/ArcanoidTiles");
            _levelLoader.LoadLevel("GameField/Levels/tiles");
        }
    }
}