using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.GameField.Block;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.Configs
{
    [CreateAssetMenu(menuName = "settings/BombsExplosionSettings")]
    public class BombsExplosionSettings : SerializedScriptableObject
    {
        [OdinSerialize] private Dictionary<BlockType, BombBehaviour> bombBehaviours;
        [OdinSerialize] private float explosionDelay;
        [OdinSerialize] private GameObject explosionEffect;
        
        public Dictionary<BlockType, BombBehaviour> BombBehaviours => bombBehaviours;
        public float ExplosionDelay => explosionDelay;
        public GameObject ExplosionEffect => explosionEffect;
    }
}