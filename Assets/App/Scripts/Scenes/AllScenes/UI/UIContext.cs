using App.Scripts.Libs.NodeArchitecture;
using UnityEngine;

namespace App.Scripts.Scenes.AllScenes.UI
{
    public class UIContext : ContextNode
    {
        [SerializeField] private LabelController labelController;

        protected override void OnConstruct()
        {
            RegisterInstance(labelController);
        }
    }
}