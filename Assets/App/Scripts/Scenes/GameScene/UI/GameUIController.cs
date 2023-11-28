using System;
using System.Collections.Generic;
using App.Scripts.Libs.DataManager;
using App.Scripts.Scenes.AllScenes.ProjectContext.Packs;
using App.Scripts.Scenes.GameScene.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.UI
{
    [Serializable]
    public class GameUIController
    {
        private readonly Dictionary<Type, Action<object, string>> _dataHandlers = new(); 
        
        private PacksController _packsController;
        private DataManager _dataManager;

        [SerializeField] private TextMeshProUGUI scoreLabel, packProgress;
        [SerializeField] private Image packImg;

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
            
            if (_packsController.StartedPack == -1) return;
            var pack = _packsController.Packs.Packs[_packsController.StartedPack];
            var packResult = _packsController.PackResults.packs[_packsController.StartedPack];

            packProgress.text = $"{packResult.nextLevel + 1} / {pack.levelCount}";
            packImg.sprite = pack.galaxyPicture;
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