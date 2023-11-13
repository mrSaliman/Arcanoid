using System.Collections.Generic;
using App.Scripts.GameScene.Game;
using App.Scripts.GameScene.Level.Tiled;
using App.Scripts.Libs.JsonResourceLoader;
using App.Scripts.Libs.ObjectPool;
using UnityEngine;

namespace App.Scripts.GameScene.Level
{
    public class LevelLoader
    {
        private Dictionary<int, Block> _tileset;
        private Dictionary<string, Block> _boundTileset;
        private ObjectPool<Block> _blockPool;

        [GameInject]
        public void Construct(GameFieldManager manager)
        {
            _boundTileset = manager.tilesetSettings.Tiles;
            _blockPool = new ObjectPool<Block>(() => new Block(), manager.levelLoaderSettings.BlockPoolSize);
        }
        
        public void LoadTileset(string path)
        {
            _tileset = new Dictionary<int, Block>();
            var tilesetData = JsonResourceLoader.LoadFromResources<TilesetData>(path);
            foreach (var tile in tilesetData.tiles)
            {
                if (_boundTileset.TryGetValue(tile.type, out var value)) _tileset[tile.id] = value;
            }
        }
        
        public Level LoadLevel(string path)
        {
            if (_tileset.Count == 0)
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
                Block boundTile = null;
                if (_tileset.TryGetValue(tile, out var value))
                {
                    boundTile = _blockPool.Get();
                    boundTile.Copy(value);
                }
                level.SetBlock(boundTile, index);
            }

            return level;
        }
    }
}