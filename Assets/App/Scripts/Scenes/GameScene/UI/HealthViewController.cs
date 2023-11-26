using System;
using System.Collections.Generic;
using App.Scripts.Configs;
using App.Scripts.Libs.DataManager;
using App.Scripts.Scenes.GameScene.Game;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace App.Scripts.Scenes.GameScene.UI
{
    [Serializable]
    public class HealthViewController
    {
        private readonly Dictionary<Type, Action<object, string>> _dataHandlers = new();
        private List<Image> _healthPoints = new();

        [SerializeField] private RectTransform hpContainer;
        [SerializeField] private Image hpPrefab;
        
        private DataManager _dataManager;
        private HealthControllerSettings _settings;
        
        private int _fullHealth, _offHealth = 0;

        [GameInject]
        public void Construct(DataManager dataManager, HealthController healthController)
        {
            _settings = healthController.Settings;
            _dataManager = dataManager;
            _dataManager.DataChanged += HandleDataChanged;
        }

        [GameInit]
        public void Init()
        {
            _dataHandlers[typeof(int)] = HandleIntData;
        }
        
        private void HandleDataChanged(object sender, DataChangedEventArgs e)
        {
            var dataType = e.NewData.GetType();

            if (!_dataHandlers.ContainsKey(dataType)) return;
            _dataHandlers[dataType].Invoke(e.NewData, e.TextID);
        }
        
        private void HandleIntData(object data, string textID)
        {
            if (textID != "hp") return;
            SetHp((int)data);
        }

        private void SetHp(int hp)
        {
            hp = Mathf.Max(hp - 1, 0);
            if (hp > _fullHealth)
            {
                for (var i = 0; i < hp - _fullHealth; i++)
                {
                    var healthPoint = Object.Instantiate(hpPrefab, hpContainer);
                    On(healthPoint);
                    _healthPoints.Add(healthPoint);
                }
                
                _fullHealth = hp;
            }

            var diff = _fullHealth - hp;

            for (var i = _offHealth - 1; i >= diff; i--)
            {
                On(_healthPoints[i]);
            }
            
            for (var i = _offHealth; i < diff; i++)
            {
                Off(_healthPoints[i]);
            }

            _offHealth = diff;
        }

        private void On(Image img)
        {
            img.color = _settings.OnHp;
        }
        
        private void Off(Image img)
        {
            img.color = _settings.OffHp;
        }
    }
}