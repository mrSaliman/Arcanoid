using System;
using System.Collections.Generic;
using App.Scripts.Libs.DataManager;
using App.Scripts.Scenes.AllScenes.ProjectContext;
using App.Scripts.Scenes.GameScene.Game;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace App.Scripts.Scenes.AllScenes.UI
{
    public class LabelController : SerializedMonoBehaviour
    {
        private readonly Dictionary<Type, Action<object, string>> _dataHandlers = new(); 
        [ShowInInspector] [OdinSerialize] private Dictionary<string, List<ControlledLabel>> _labels;

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
            HandleLanguageChanged();
        }
        
        private void HandleDataChanged(object sender, DataChangedEventArgs e)
        {
            var dataType = e.NewData.GetType();

            if (!_dataHandlers.ContainsKey(dataType)) return;
            _dataHandlers[dataType].Invoke(e.NewData, e.TextID);
        }

        private void HandleStringData(object data, string textID)
        {
            if (!_labels.ContainsKey(textID)) return;
            foreach (var label in _labels[textID])
            {
                label.SetData((string)data);
            }
        }

        private void HandleLanguageChanged()
        {
            foreach (var labelSet in _labels)
            {
                var textToSet = _localizationManager.GetLocalizedString(labelSet.Key);
                foreach (var label in labelSet.Value)
                {
                    label.SetText(textToSet);
                }
            }
        }

        public void AddLabel(string key, ControlledLabel label)
        {
            if (!_labels.ContainsKey(key)) _labels[key] = new List<ControlledLabel>();
            _labels[key].Add(label);
            label.SetText(_localizationManager.GetLocalizedString(key));
        }

        public void RemoveLabel(string key, ControlledLabel label)
        {
            if (!_labels.ContainsKey(key)) return;
            _labels[key].Remove(label);
        }
    }
}