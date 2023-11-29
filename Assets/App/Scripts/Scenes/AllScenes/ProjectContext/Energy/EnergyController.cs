using System;
using System.IO;
using App.Scripts.Configs;
using App.Scripts.Libs.JsonDataService;
using App.Scripts.Scenes.GameScene.Game;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.AllScenes.ProjectContext.Energy
{
    public class EnergyController : MonoBehaviour
    {
        private EnergyInfo _lastInfo;
        
        [SerializeField] private EnergySettings settings;
        private TimeSpan _gainPeriod;

        public EnergyInfo LastInfo => _lastInfo;
        public EnergySettings Settings => settings;

        [GameInit]
        public void Init()
        {
            _gainPeriod = new TimeSpan(settings.GainPeriod.x, settings.GainPeriod.y, settings.GainPeriod.z);
            if (_lastInfo == null && !JsonDataService.TryLoadData(settings.SavePath, out _lastInfo))
            {
                _lastInfo = new EnergyInfo
                {
                    Amount = settings.StartEnergy,
                    NextGainTime = _gainPeriod,
                    TimePoint = DateTime.Now
                };
            }
        }

        public void UpdateInfo()
        {
            var currentTime = DateTime.Now;
            var span = currentTime - _lastInfo.TimePoint;
            if (span >= _lastInfo.NextGainTime)
            {
                span -= _lastInfo.NextGainTime;
                var addTimes = (int)(span / _gainPeriod);
                _lastInfo.Amount += (addTimes + 1) * settings.GainAmount;
                if (_lastInfo.Amount > settings.MaxEnergy) _lastInfo.Amount = settings.MaxEnergy;
                _lastInfo.NextGainTime = _gainPeriod - (span - addTimes * _gainPeriod);
            }
            else
            {
                _lastInfo.NextGainTime -= span;
            }
            _lastInfo.TimePoint = currentTime;
        }

        public bool UseEnergy(int amount)
        {
            UpdateInfo();
            if (amount > _lastInfo.Amount) return false;
            _lastInfo.Amount -= amount;
            return true;
        }

        public bool UsePlayEnergy()
        {
            return UseEnergy(settings.PlayCost);
        }

        public void AddEnergy(int amount)
        {
            UpdateInfo();
            _lastInfo.Amount += amount;
            if (_lastInfo.Amount > settings.MaxEnergy) _lastInfo.Amount = settings.MaxEnergy;
        }

        public void AddWinEnergy()
        {
            AddEnergy(settings.WinGainAmount);
        }

        public void SaveInfo()
        {
            UpdateInfo();
            JsonDataService.SaveData(settings.SavePath, _lastInfo);
        }

        [Button]
        public void DeleteSave()
        {
            var path = Path.Combine(Application.persistentDataPath, settings.SavePath);
            if (File.Exists(path)) File.Delete(path);
            _lastInfo = null;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            SaveInfo();
        }

        private void OnApplicationQuit()
        {
            SaveInfo();
        }
    }
}