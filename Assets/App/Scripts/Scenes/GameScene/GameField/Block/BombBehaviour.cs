using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.GameField.Block
{
    [Serializable]
    public class BombBehaviour
    {
        public bool isChainBomb;

        [Title("Format: x : y : numberOfMoves(-1 if infinity)")]
        public List<Vector3Int> moves;

        public int damage;
    }
}