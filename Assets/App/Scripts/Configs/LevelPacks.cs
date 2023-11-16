using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.Configs
{
    [CreateAssetMenu(menuName = "settings/LevelPacks")]
    public class LevelPacks : SerializedScriptableObject
    {
        [FilePath(ParentFolder = "Assets/App/Resources", Extensions = "json", IncludeFileExtension = false)]
        [OdinSerialize] private string levelPath;

        public string LevelPath => levelPath;
    }
}