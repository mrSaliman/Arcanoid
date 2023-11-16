using System.Collections.Generic;
using App.Scripts.GameScene.GameField;
using App.Scripts.GameScene.GameField.Model;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.Configs
{
    [CreateAssetMenu(menuName = "settings/TilesetSettings")]
    public class TilesetSettings : SerializedScriptableObject
    {
        [OdinSerialize] [ShowInInspector] private Dictionary<string, BlockSpriteAssociation> _tiles;
        
        public Dictionary<string, BlockSpriteAssociation> Tiles => _tiles;
    }
}