using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace App.Scripts.Configs
{
    [CreateAssetMenu(menuName = "settings/EnergySettings")]
    public class EnergySettings : SerializedScriptableObject
    {
        [Title("HH:MM:SS format."), ValidateInput("CheckTime", "Incorrect time format!"), OdinSerialize]
        private Vector3Int gainPeriod;

        [MinValue(0), OdinSerialize] private int gainAmount;
        [MinValue(0), OdinSerialize] private int winGainAmount;

        [MinValue(0), MaxValue("$maxEnergy"), OdinSerialize]
        private int playCost;

        [MinValue(0), OdinSerialize] private int maxEnergy;

        [MinValue(0), MaxValue("$maxEnergy"), OdinSerialize]
        private int startEnergy;
        
        [SerializeField] private string savePath;

        public Vector3Int GainPeriod => gainPeriod;
        public int GainAmount => gainAmount;
        public int WinGainAmount => winGainAmount;
        public int PlayCost => playCost;
        public int MaxEnergy => maxEnergy;
        public int StartEnergy => startEnergy;
        public string SavePath => savePath;

        private bool CheckTime(Vector3Int time)
        {
            return time is { x: >= 0, y: >= 0 } and { z: >= 0, y: <= 59 } and { z: <= 59, magnitude: > 0 };
        }
    }
}