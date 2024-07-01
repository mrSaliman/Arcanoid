using System;
using App.Scripts.Configs;
using App.Scripts.Libs.DataManager;
using App.Scripts.Scenes.AllScenes.ProjectContext.Packs;
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
        private GameManager _gameManager;

        [GameInject]
        public void Construct(DataManager dataManager, GameManager gameManager)
        {
            _dataManager = dataManager;
            _gameManager = gameManager;
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

        public int DealDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth > settings.StartHp) _currentHealth = settings.StartHp;
            if (_currentHealth < 0) _currentHealth = 0;
            _dataManager.ModifyData("hp", _currentHealth);
            return _currentHealth;
        }
    }
}