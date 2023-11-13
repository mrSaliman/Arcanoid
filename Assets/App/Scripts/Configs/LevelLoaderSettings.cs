using UnityEngine;

namespace App.Scripts.Configs
{
    [CreateAssetMenu(menuName = "settings/LevelLoaderSettings")]
    public class LevelLoaderSettings : ScriptableObject
    {
        [SerializeField] private int blockPoolSize;

        public int BlockPoolSize => blockPoolSize;
    }
}