using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.GameField;
using App.Scripts.Scenes.GameScene.GameField.Block;
using App.Scripts.Scenes.GameScene.GameField.Boosts;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.Configs
{
    [CreateAssetMenu(menuName = "settings/BoostSettings")]
    public class BoostsSettings : SerializedScriptableObject
    {
        [OdinSerialize] private Dictionary<BlockType, BoostSettings> boosts;
        [OdinSerialize] private float boostSpeed;
        
        public Dictionary<BlockType, BoostSettings> Boosts => boosts;
        public float BoostSpeed => boostSpeed;
    }
}