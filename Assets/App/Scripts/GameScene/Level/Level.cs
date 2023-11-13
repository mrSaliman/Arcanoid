using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.GameScene.Level
{
    public class Level
    {
        private readonly int _height;
        private readonly int _width;
        public readonly Block[] Blocks;
        

        public Level(int height, int width)
        {
            _height = height;
            _width = width;
            Blocks = new Block[height * width];
        }

        public void SetBlock(Block block, int x, int y)
        {
            Blocks[x + y * _width] = block;
            if (block is null) return;
            block.OnHealthDepleted += BlockHealthDepletedHandler; 
        }
        
        public void SetBlock(Block block, int index)
        {
            Blocks[index] = block;
            if (block is null) return;
            block.OnHealthDepleted += BlockHealthDepletedHandler; 
        }

        public void RemoveBlock(int x, int y)
        {
            Blocks[x + y * _width] = null;
        }

        private void BlockHealthDepletedHandler(Block block)
        {
            Debug.Log("Block ded");
        }

        public void SubscribeToBlock(int blockIndex, Action<Block> callback)
        {
            if (blockIndex >= 0 && blockIndex < Blocks.Length)
            {
                Blocks[blockIndex].OnHealthDepleted += callback;
            }
            else
            {
                Debug.LogError("Block index out of range.");
            }
        }

        public void UnsubscribeFromBlock(int blockIndex, Action<Block> callback)
        {
            if (blockIndex >= 0 && blockIndex < Blocks.Length)
            {
                Blocks[blockIndex].OnHealthDepleted -= callback;
            }
            else
            {
                Debug.LogError("Block index out of range.");
            }
        }
    }
}