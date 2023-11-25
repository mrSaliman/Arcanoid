using System;
using App.Scripts.Configs;
using App.Scripts.Libs.DataManager;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Game
{
    [Serializable]
    public class HealthController
    {
        private int _currentHealth;

        [SerializeField] private HealthControllerSettings settings;

        public HealthControllerSettings Settings => settings;

        private DataManager _dataManager;

        [GameInject]
        public void Construct(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        [GameInit]
        public void Init()
        {
            _currentHealth = settings.StartHp;
        }

        [GameStart]
        public void Start()
        {
            _dataManager.ModifyData("hp", _currentHealth);
        }

        public void DealDamage(int damage)
        {
            _currentHealth -= damage;
            _dataManager.ModifyData("hp", _currentHealth);
            if (_currentHealth <= 0)
            {
                //TODO end health behaviour
            }
        }
    }
}