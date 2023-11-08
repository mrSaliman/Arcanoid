using System;
using System.Collections.Generic;
using System.IO;
using App.Scripts.GameScene.Game;
using UnityEngine;

namespace App.Scripts.AllScenes.ProjectContext
{
    [Serializable]
    public class LocalizationManager
    {
        private readonly Dictionary<SystemLanguage, Dictionary<string, string>> _translations = new();
        [SerializeField] private SystemLanguage currentLanguage;

        public SystemLanguage CurrentLanguage => currentLanguage;

        public event Action LanguageChanged;
        
        [GameInit]
        public void Init()
        {
            ChangeLanguage(currentLanguage);
        }
        
        private void UploadLanguage(SystemLanguage language)
        {
            if (_translations.ContainsKey(language)) return;
            var csvFile = $"Localization/{language}";
            var csvText = Resources.Load<TextAsset>(csvFile);

            if (csvText == null) return;
            var translation = new Dictionary<string, string>();
            var reader = new StringReader(csvText.text);

            while (reader.Peek() > -1)
            {
                var values = reader.ReadLine()?.Split('|');

                if (values is null)
                {
                    Debug.LogError("Localization line is null");
                    continue;
                }
                if (values.Length < 2)
                {
                    Debug.LogError("Can't parse localization line. Wrong length.");
                    continue;
                }
                translation[values[0]] = values[1];
            }

            _translations[language] = translation;
            currentLanguage = language;
        }

        public string GetLocalizedString(string key)
        {
            if (_translations.ContainsKey(currentLanguage) && _translations[currentLanguage].ContainsKey(key))
            {
                return _translations[currentLanguage][key];
            }

            return "Localization Error: " + key;
        }
        
        public void ChangeLanguage(SystemLanguage language)
        {
            UploadLanguage(language);
            if (language != currentLanguage) ClearLanguage(currentLanguage);
            currentLanguage = language;
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