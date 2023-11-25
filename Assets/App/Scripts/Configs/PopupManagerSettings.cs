using App.Scripts.Scenes.AllScenes.ProjectContext.Pop_Up;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts.Configs
{
    [CreateAssetMenu(menuName = "settings/PopupManagerSettings")]
    public class PopupManagerSettings : SerializedScriptableObject
    {
        [OdinSerialize] private Popup popupPrefab;
        [OdinSerialize] private PopupLabel popupLabelPrefab;
        [OdinSerialize] private PopupButton popupButtonPrefab;
        
        public Popup PopupPrefab => popupPrefab;
        public PopupLabel PopupLabelPrefab => popupLabelPrefab;
        public PopupButton PopupButtonPrefab => popupButtonPrefab;
    }
}