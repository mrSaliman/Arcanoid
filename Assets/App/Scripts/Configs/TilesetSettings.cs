using System.Collections.Generic;
using App.Scripts.GameScene.Level;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.Configs
{
    [CreateAssetMenu(menuName = "settings/TilesetSettings")]
    public class TilesetSettings : SerializedScriptableObject
    {
        [OdinSerialize] [ShowInInspector] private Dictionary<string, Block> _tiles;
        
        public Dictionary<string, Block> Tiles => _tiles;
    }
}