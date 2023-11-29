using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Scenes.GameScene.GameField.Boosts
{
    [Serializable]
    public class BoostSettings
    {
        public Sprite sprite;
        public float duration;
        public float impact;
    }
}