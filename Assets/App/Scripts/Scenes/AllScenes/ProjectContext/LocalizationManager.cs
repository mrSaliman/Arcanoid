using System;
using System.Collections.Generic;
using App.Scripts.Configs;
using App.Scripts.Libs.JsonDataService;
using App.Scripts.Scenes.GameScene.Game;
using UnityEngine;

namespace App.Scripts.Scenes.AllScenes.ProjectContext
{
    [Serializable]
    public class LocalizationManager
    {
        [SerializeField] private LocalizationSettings localizationSettings;

        public LocalizationSettings LocalizationSettings => localizationSettings;

        private readonly Dictionary<SystemLanguage, Dictionary<string, string>> _translations = new(); 
        private SystemLanguage _currentLanguage;

        public event Action LanguageChanged;
        
        public void Init()
        {
            _currentLanguage = localizationSettings.StartLanguage;
            UploadLanguage(_currentLanguage);
        }
        
        private void UploadLanguage(SystemLanguage language)
        {
            if (_translations.ContainsKey(language)) return;
            _translations[language] =
                JsonDataService.LoadFromResources<Dictionary<string, string>>(localizationSettings.LocalesFolder +
                    "/" + language);
            _currentLanguage = language;
        }

        public string GetLocalizedString(string key)
        {
            if (_translations.ContainsKey(_currentLanguage) && _translations[_currentLanguage].ContainsKey(key))
            {
                return _translations[_currentLanguage][key];
            }

            return "Localization Error: " + key;
        }
        
        public void ChangeLanguage(SystemLanguage language)
        {
            UploadLanguage(language);
            if (language != _currentLanguage) ClearLanguage(_currentLanguage);
            _currentLanguage = language;
            LanguageChanged?.Invoke();
        }

        private void ClearLanguage(SystemLanguage language)
        {
            if (_translations.ContainsKey(language))
            {
                _translations.Remove(language);
            }
        }
    }
}