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
        [HideReferenceObjectPicker]
        [OdinSerialize] private List<LevelPack> packs;
    }
}