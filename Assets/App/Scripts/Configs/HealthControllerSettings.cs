using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.Configs
{
    [CreateAssetMenu(menuName = "settings/HealthControllerSettings")]
    public class HealthControllerSettings : SerializedScriptableObject
    {
        [OdinSerialize] private int startHp;
        [OdinSerialize] private Color offHp;
        [OdinSerialize] private Color onHp;

        public int StartHp => startHp;
        public Color OffHp => offHp;
        public Color OnHp => onHp;
    }
}