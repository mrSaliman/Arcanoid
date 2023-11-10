using System;
using System.Collections.Generic;
using App.Scripts.AllScenes.ProjectContext;
using App.Scripts.GameScene.Game;
using App.Scripts.Libs.DataManager;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.AllScenes.UI
{
    public class LabelController : SerializedMonoBehaviour
    {
        private Dictionary<Type, Action<object, string>> _dataHandlers = new(); 
        [ShowInInspector] [OdinSerialize] private Dictionary<string, LocalizedLabel> labels;

        private LocalizationManager _localizationManager;
        private DataManager _dataManager;

        [GameInject]
        public void Construct(LocalizationManager localizationManager, DataManager dataManager)
        {
            _dataManager = dataManager;
            _localizationManager = localizationManager;
            _dataManager.DataChanged += HandleDataChanged;
            _localizationManager.LanguageChanged += HandleLanguageChanged;
        }

        [GameInit]
        public void Init()
        {
            _dataHandlers[typeof(string)] = HandleStringData;
        }
        
        private void HandleDataChanged(object sender, DataChangedEventArgs e)
        {
            var dataType = e.NewData.GetType();

            if (!_dataHandlers.ContainsKey(dataType)) return;
            _dataHandlers[dataType].Invoke(e.NewData, e.TextID);
        }

        private void HandleStringData(object data, string textID)
        {
            if (!labels.ContainsKey(textID)) return;
            labels[textID].SetData((string)data);
        }

        private void HandleLanguageChanged()
        {
            foreach (var label in labels)
            {
                label.Value.SetText(_localizationManager.GetLocalizedString(label.Key));
            }
        }
    }
}