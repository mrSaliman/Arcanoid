using UnityEngine;

namespace App.Scripts.Scenes.GameScene.GameField.Level
{
    public class Level
    {
        public readonly Block.Block[] Blocks;
        public readonly int[] Tags;
        
        public int Height { get; }

        public int Width { get; }

        public Level(int height, int width)
        {
            Height = height;
            Width = width;
            Blocks = new Block.Block[height * width];
            Tags = new int[height * width];
        }

        public Block.Block GetBlock(int x, int y)
        {
            if (Fits(x, y)) return Blocks[x + y * Width];
            Debug.LogError("Index is out of range");
            return null;
        }
        
        public int GetTag(int x, int y)
        {
            if (Fits(x, y)) return Tags[x + y * Width];
            Debug.LogError("Index is out of range");
            return -1;
        }

        public void SetBlock(Block.Block block, int x, int y)
        {
            if (!Fits(x, y))
            {
                Debug.LogError("Index is out of range");
                return;
            }
            Blocks[x + y * Width] = block;
            if (block is null) return;
        }
        
        public void SetBlock(Block.Block block, int index)
        {
            if (!Fits(index))
            {
                Debug.LogError("Index is out of range");
                return;
            }
            Blocks[index] = block;
            if (block is null) return;
        }

        public void RemoveBlock(int x, int y)
        {
            if (!Fits(x, y))
            {
                Debug.LogError("Index is out of range");
                return;
            }
            
            Blocks[x + y * Width] = null;
        }

        private bool Fits(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        private bool Fits(int index)
        {
            return index >= 0 && index <= Blocks.Length;
        }
    }
}