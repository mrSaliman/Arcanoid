using System;
using Sirenix.OdinInspector;

namespace App.Scripts.Scenes.GameScene.GameField.Level
{
    [Serializable]
    public class LevelPath
    {
        [FilePath(ParentFolder = "Assets/App/Resources", Extensions = "json", IncludeFileExtension = false)]
        public string path;
    }
}