using UnityEngine;

namespace App.Scripts.AllScenes.ProjectContext
{
    [CreateAssetMenu(menuName = "settings/LocalizationSettings")]
    public class LocalizationSettings : ScriptableObject
    {
        [SerializeField] private SystemLanguage startLanguage;
        [SerializeField] private string localesFolder;

        public SystemLanguage StartLanguage => startLanguage;
        public string LocalesFolder => localesFolder;
    }
}