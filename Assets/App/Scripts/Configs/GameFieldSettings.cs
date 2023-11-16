using App.Scripts.GameScene.GameField.View;
using UnityEngine;

namespace App.Scripts.Configs
{
    [CreateAssetMenu(menuName = "settings/GameFieldSettings")]
    public class GameFieldSettings : ScriptableObject
    {
        [SerializeField] [Range(0, 1)] private float topIndentationPercent;
        [SerializeField] private float horizontalIndentation;
        [SerializeField] private float betweenBlockIndentation;
        [SerializeField] private Vector2 defaultBlockSize;
        [SerializeField] private BlockView blockViewPrefab;
        
        public float TopIndentationPercent => topIndentationPercent;
        public float HorizontalIndentation => horizontalIndentation;
        public float BetweenBlockIndentation => betweenBlockIndentation;

        public Vector2 DefaultBlockSize => defaultBlockSize;
        public BlockView BlockViewPrefab => blockViewPrefab;
    }
}