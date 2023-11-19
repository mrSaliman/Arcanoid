using System;
using App.Scripts.GameScene.GameField.Block;
using UnityEngine;

namespace App.Scripts.Configs
{
    [Serializable]
    public class BlockSpriteAssociation
    {
        public Block block;
        public Sprite sprite;
    }
}