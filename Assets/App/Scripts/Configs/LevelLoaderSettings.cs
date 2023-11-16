using UnityEngine;

namespace App.Scripts.Configs
{
    [CreateAssetMenu(menuName = "settings/LevelLoaderSettings")]
    public class LevelLoaderSettings : ScriptableObject
    {
        [SerializeField] private int blockPoolSize;
        [SerializeField] private string tilesetPath;
        [SerializeField] private string levelsFolder;
        
        public int BlockPoolSize => blockPoolSize;
        public string TilesetPath => tilesetPath;
        public string LevelsFolder => levelsFolder;
    }
}