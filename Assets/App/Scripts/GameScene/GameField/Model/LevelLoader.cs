using System.Collections.Generic;
using App.Scripts.Configs;
using App.Scripts.GameScene.Game;
using App.Scripts.GameScene.GameField.Model.Tiled;
using App.Scripts.Libs.JsonResourceLoader;
using App.Scripts.Libs.ObjectPool;
using UnityEngine;

namespace App.Scripts.GameScene.GameField.Model
{
    public class LevelLoader
    {
        private ObjectPool<Block> _blockPool;

        public Dictionary<int, BlockSpriteAssociation> Tileset { get; private set; }

        private LevelLoaderSettings _settings;

        [GameInject]
        public void Construct(GameFieldManager manager)
        {
            _settings = manager.levelLoaderSettings;
            LoadTileset(manager.tilesetSettings);
            _blockPool = new ObjectPool<Block>(() => new Block(), _settings.BlockPoolSize);
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
        
        public Level LoadLevel(string name)
        {
            if (Tileset.Count == 0)
            {
                Debug.LogError("Tileset is not loaded!");
                return null;
            }
            
            var map = JsonResourceLoader.LoadFromResources<MapData>(_settings.LevelsFolder + name);
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
                Block boundTile = null;
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