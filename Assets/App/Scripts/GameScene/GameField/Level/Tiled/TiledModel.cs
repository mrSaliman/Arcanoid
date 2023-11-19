using System;

namespace App.Scripts.GameScene.GameField.Level.Tiled
{
    [Serializable]
    public class MapData
    {
        public int compressionlevel;
        public int height;
        public bool infinite;
        public Layer[] layers;
        public int nextlayerid;
        public int nextobjectid;
        public string orientation;
        public string renderorder;
        public string tiledversion;
        public int tileheight;
        public Tileset[] tilesets;
        public int tilewidth;
        public string type;
        public string version;
        public int width;
    }

    [Serializable]
    public class Layer
    {
        public int[] data;
        public int height;
        public int id;
        public string name;
        public float opacity;
        public string type;
        public bool visible;
        public int width;
        public int x;
        public int y;
    }

    [Serializable]
    public class Tileset
    {
        public int firstgid;
        public string source;
    }

    
    [Serializable]
    public class TilesetData
    {
        public int columns;
        public GridData grid;
        public int margin;
        public string name;
        public int spacing;
        public int tilecount;
        public string tiledversion;
        public int tileheight;
        public TileData[] tiles;
        public int tilewidth;
        public string type;
        public string version;
    }

    [Serializable]
    public class GridData
    {
        public int height;
        public string orientation;
        public int width;
    }

    [Serializable]
    public class TileData
    {
        public int id;
        public string image;
        public int imageheight;
        public int imagewidth;
        public string type;
    }

}