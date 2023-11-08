using System;
using System.Collections.Generic;
using App.Scripts.AllScenes.ProjectContext;
using App.Scripts.GameScene.Game;
using UnityEngine;

namespace App.Scripts.AllScenes.UI
{
    [Serializable]
    public class LabelsLocalizationManager
    {
        [SerializeField] private List<LocalizedLabel> labels;

        private LocalizationManager _localizationManager;

        [GameInject]
        public void Construct(LocalizationManager localizationManager)
        {
            _localizationManager = localizationManager;
            _localizationManager.LanguageChanged += OnLanguageChanged;
        }

        private void OnLanguageChanged()
        {
            foreach (var label in labels)
            {
                label.SetText(_localizationManager.GetLocalizedString(label.Key));
            }
        }
    }
}