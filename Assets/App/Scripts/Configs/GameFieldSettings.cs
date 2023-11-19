using App.Scripts.GameScene.GameField.Ball;
using App.Scripts.GameScene.GameField.Block;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.Configs
{
    [CreateAssetMenu(menuName = "settings/GameFieldSettings")]
    public class GameFieldSettings : SerializedScriptableObject
    {
        [OdinSerialize] [PropertyRange(0, 1)] private float topIndentationPercent;
        [OdinSerialize] private float horizontalIndentation;
        [OdinSerialize] private float betweenBlockIndentation;
        [OdinSerialize] private Vector2 defaultBlockSize;
        [OdinSerialize] private BlockView blockViewPrefab;
        [OdinSerialize] private BallView ballViewPrefab;
        
        [FilePath(ParentFolder = "Assets/App/Resources", Extensions = "json", IncludeFileExtension = false)]
        [OdinSerialize]
        private string debugLevel;
        
        public float TopIndentationPercent => topIndentationPercent;
        public float HorizontalIndentation => horizontalIndentation;
        public float BetweenBlockIndentation => betweenBlockIndentation;
        public Vector2 DefaultBlockSize => defaultBlockSize;
        public BlockView BlockViewPrefab => blockViewPrefab;
        public BallView BallViewPrefab => ballViewPrefab;
        public string DebugLevel => debugLevel;
    }
}