using UnityEngine;

namespace App.Scripts.GameScene.GameField.Model
{
    public class Level
    {
        private readonly int _height;
        private readonly int _width;
        public readonly Block[] Blocks;
        public readonly int[] Tags;
        
        public int Height => _height;
        public int Width => _width;

        public Level(int height, int width)
        {
            _height = height;
            _width = width;
            Blocks = new Block[height * width];
            Tags = new int[height * width];
        }

        public Block GetBlock(int x, int y)
        {
            if (Fits(x, y)) return Blocks[x + y * _width];
            Debug.LogError("Index is out of range");
            return null;
        }
        
        public int GetTag(int x, int y)
        {
            if (Fits(x, y)) return Tags[x + y * _width];
            Debug.LogError("Index is out of range");
            return -1;
        }

        public void SetBlock(Block block, int x, int y)
        {
            if (!Fits(x, y))
            {
                Debug.LogError("Index is out of range");
                return;
            }
            Blocks[x + y * _width] = block;
            if (block is null) return;
            block.OnHealthDepleted += BlockHealthDepletedHandler; 
        }
        
        public void SetBlock(Block block, int index)
        {
            if (!Fits(index))
            {
                Debug.LogError("Index is out of range");
                return;
            }
            Blocks[index] = block;
            if (block is null) return;
            block.OnHealthDepleted += BlockHealthDepletedHandler; 
        }

        public void RemoveBlock(int x, int y)
        {
            if (!Fits(x, y))
            {
                Debug.LogError("Index is out of range");
                return;
            }
            
            Blocks[x + y * _width] = null;
        }

        private bool Fits(int x, int y)
        {
            return x >= 0 && x < _width && y >= 0 && y < _height;
        }

        private bool Fits(int index)
        {
            return index >= 0 && index <= Blocks.Length;
        }

        private void BlockHealthDepletedHandler()
        {
            Debug.Log("Block ded");
        }
    }
}