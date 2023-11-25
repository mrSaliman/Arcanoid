using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace App.Scripts.Scenes.GameScene.GameField.Level
{
    [Serializable]
    public class LevelPack
    {
        public string name;
        [HideReferenceObjectPicker]
        [LabelText("$name")]
        public List<LevelPath> pack;
    }
}