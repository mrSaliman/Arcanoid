using App.Scripts.Libs.NodeArchitecture;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.AllScenes.UI
{
    public class UIContext : ContextNode
    {
        [FormerlySerializedAs("labelsLocalizationManager")] [SerializeField] private LabelController labelController = new();

        protected override void OnConstruct()
        {
            RegisterInstance(labelController);
        }
    }
}