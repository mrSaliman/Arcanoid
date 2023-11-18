using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.Configs
{
    [CreateAssetMenu(menuName = "settings/PlatformSettings")]
    public class PlatformSettings : SerializedScriptableObject
    {
        [OdinSerialize] private float platformSpeed;
        [MinMaxSlider(0, 2)]
        [OdinSerialize] private Vector2 platformSize;
        [PropertyRange(0, 1)]
        [OdinSerialize] private float bottomIndentation;
        [OdinSerialize] private float smoothness;
        
        public float PlatformSpeed => platformSpeed;
        public Vector2 PlatformSize => platformSize;
        public float BottomIndentation => bottomIndentation;

        public float Smoothness => smoothness;
    }
}