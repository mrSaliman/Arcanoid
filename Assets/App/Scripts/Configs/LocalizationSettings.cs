using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.Configs
{
    [CreateAssetMenu(menuName = "settings/LocalizationSettings")]
    public class LocalizationSettings : SerializedScriptableObject
    {
        [OdinSerialize] private SystemLanguage startLanguage;
        
        [OdinSerialize]
        [FolderPath(ParentFolder = "Assets/App/Resources")]
        private string localesFolder;

        [OdinSerialize]
        [ListDrawerSettings(CustomAddFunction = "AddLanguage")]
        [TableList]
        private List<AvailableLanguage> availableLanguages;

        
        public SystemLanguage StartLanguage => startLanguage;
        public string LocalesFolder => localesFolder;
        public List<AvailableLanguage> AvailableLanguages => availableLanguages;


        [Serializable]
        public class AvailableLanguage
        {
            public SystemLanguage Language;
            [OdinSerialize] [HideInInspector] private LocalizationSettings _parent;

            public AvailableLanguage(LocalizationSettings parent)
            {
                _parent = parent;
            }


            [Button]
            public void SetThisLanguage()
            {
                _parent.startLanguage = Language;
            }
        }

        private AvailableLanguage AddLanguage()
        {
            return new AvailableLanguage(this);
        }
    }
}