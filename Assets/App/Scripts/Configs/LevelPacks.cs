using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.GameField.Level;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.Configs
{
    [CreateAssetMenu(menuName = "settings/LevelPacks")]
    public class LevelPacks : SerializedScriptableObject
    {
        [FolderPath(ParentFolder = "Assets/App/Resources")]
        [OdinSerialize] private string levelsFolder;
        
        [HideReferenceObjectPicker]
        [OdinSerialize] private List<LevelPack> packs;
        
        public List<LevelPack> Packs => packs;
        public string LevelsFolder => levelsFolder;
    }
}