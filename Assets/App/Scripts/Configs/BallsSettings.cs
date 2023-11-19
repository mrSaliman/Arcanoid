using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.Configs
{
    [CreateAssetMenu(menuName = "settings/BallsSettings")]
    public class BallsSettings : SerializedScriptableObject
    {
        [OdinSerialize] private int ballPoolSize;
        [OdinSerialize] [PropertyRange(0, 90)] private float minReflectionAngle;
        [OdinSerialize] [MinMaxSlider(0, 30)] private Vector2 ballSpeed;

        public int BallPoolSize => ballPoolSize;
        public float MinReflectionAngle => minReflectionAngle;
        public Vector2 BallSpeed => ballSpeed;
    }
}