using System;
using System.Collections.Generic;
using App.Scripts.Libs.DataManager;
using App.Scripts.Scenes.AllScenes.ProjectContext.Packs;
using App.Scripts.Scenes.GameScene.Game;
using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.UI
{
    [Serializable]
    public class GameUIController
    {
        private readonly Dictionary<Type, Action<object, string>> _dataHandlers = new(); 
        
        private PacksController _packsController;
        private DataManager _dataManager;

        [SerializeField] private TextMeshProUGUI scoreLabel, packProgress;

        [GameInject]
        public void Construct(PacksController packsController, DataManager dataManager)
        {
            _packsController = packsController;
            _dataManager = dataManager;
            _dataManager.DataChanged += HandleDataChanged;
        }

        [GameInit]
        public void Init()
        {
            _dataHandlers[typeof(float)] = HandleFloatData;
            scoreLabel.text = "0%";
            
            if (_packsController.StartedLevel == -1) return;
            var pack = _packsController.Packs.Packs[_packsController.StartedLevel];
            var packResult = _packsController.PackResults.packs[_packsController.StartedLevel];

            packProgress.text = $"{packResult.progress} / {pack.levels.Count}";
        }
        
        private void HandleDataChanged(object sender, DataChangedEventArgs e)
        {
            var dataType = e.NewData.GetType();

            if (!_dataHandlers.ContainsKey(dataType)) return;
            _dataHandlers[dataType].Invoke(e.NewData, e.TextID);
        }
        
        private void HandleFloatData(object data, string textID)
        {
            if (textID != "score") return;
            scoreLabel.text = $"{(int)((float)data * 100f)}%";
        }
    }
}