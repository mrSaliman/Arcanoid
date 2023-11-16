using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.Configs
{
    [CreateAssetMenu(menuName = "settings/LevelLoaderSettings")]
    public class LevelLoaderSettings : SerializedScriptableObject
    {
        [OdinSerialize] private int blockPoolSize;
        [FilePath(ParentFolder = "Assets/App/Resources", Extensions = "json", IncludeFileExtension = false)]
        [OdinSerialize] private string tilesetPath;
        
        public int BlockPoolSize => blockPoolSize;
        public string TilesetPath => tilesetPath;
    }
}