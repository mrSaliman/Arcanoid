using App.Scripts.Libs.NodeArchitecture;
using UnityEngine;

namespace App.Scripts.AllScenes.UI
{
    public class UIContext : ContextNode
    {
        [SerializeField] private LabelsLocalizationManager labelsLocalizationManager = new();

        protected override void OnConstruct()
        {
            RegisterInstance(labelsLocalizationManager);
        }
    }
}