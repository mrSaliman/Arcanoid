using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.GameField.Level
{
    [Serializable]
    public class LevelPack
    {
        public string name;
        [PreviewField] public Sprite galaxyPicture;

        public Color buttonColor;

        [HideReferenceObjectPicker] [LabelText("$name")]
        public List<LevelPath> pack;
    }
}