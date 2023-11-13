using System;
using System.Collections.Generic;
using App.Scripts.Configs;
using App.Scripts.GameScene.Game;
using App.Scripts.Libs.JsonResourceLoader;
using UnityEngine;

namespace App.Scripts.AllScenes.ProjectContext
{
    [Serializable]
    public class LocalizationManager
    {
        [SerializeField] private LocalizationSettings localizationSettings;
        
        private readonly Dictionary<SystemLanguage, Dictionary<string, string>> _translations = new(); 
        private SystemLanguage _currentLanguage;

        public SystemLanguage CurrentLanguage => _currentLanguage;

        public event Action LanguageChanged;
        
        [GameInit]
        public void Init()
        {
            _currentLanguage = localizationSettings.StartLanguage;
            ChangeLanguage(_currentLanguage);
        }
        
        private void UploadLanguage(SystemLanguage language)
        {
            if (_translations.ContainsKey(language)) return;
            _translations[language] =
                JsonResourceLoader.LoadFromResources<Dictionary<string, string>>(localizationSettings.LocalesFolder +
                    language);
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