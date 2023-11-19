using System.Collections.Generic;
using App.Scripts.Configs;
using App.Scripts.GameScene.Game;
using App.Scripts.GameScene.GameField.Level.Tiled;
using App.Scripts.Libs.JsonResourceLoader;
using App.Scripts.Libs.ObjectPool;
using UnityEngine;

namespace App.Scripts.GameScene.GameField.Level
{
    public class LevelLoader
    {
        private ObjectPool<Block.Block> _blockPool;

        public Dictionary<int, BlockSpriteAssociation> Tileset { get; private set; }

        private GameFieldManager _gameFieldManager;

        private LevelLoaderSettings _settings;

        [GameInject]
        public void Construct(GameFieldManager manager)
        {
            _settings = manager.levelLoaderSettings;
            manager.BlockPool = new ObjectPool<Block.Block>(() => new Block.Block(), _settings.BlockPoolSize);
            _blockPool = manager.BlockPool;
            LoadTileset(manager.tilesetSettings);
        }

        private void LoadTileset(TilesetSettings tilesetSettings)
        {
            Tileset = new Dictionary<int, BlockSpriteAssociation>();
            var tilesetData = JsonResourceLoader.LoadFromResources<TilesetData>(_settings.TilesetPath);
            foreach (var tile in tilesetData.tiles)
            {
                if (tilesetSettings.Tiles.TryGetValue(tile.type, out var value)) Tileset[tile.id] = value;
            }
        }
        
        public Level LoadLevel(string path)
        {
            if (Tileset.Count == 0)
            {
                Debug.LogError("Tileset is not loaded!");
                return null;
            }
            
            var map = JsonResourceLoader.LoadFromResources<MapData>(path);
            if (map.layers.Length < 1)
            {
                Debug.LogError("Map has no layers!");
                return null;
            }

            var layer = map.layers[0];
            var level = new Level(layer.height, layer.width);
            for (var index = 0; index < layer.data.Length; index++)
            {
                var tile = layer.data[index] - 1;
                Block.Block boundTile = null;
                if (Tileset.TryGetValue(tile, out var value))
                {
                    boundTile = _blockPool.Get();
                    boundTile.Copy(value.block);
                    level.Tags[index] = tile;
                }
                level.SetBlock(boundTile, index);
            }

            return level;
        }
    }
}