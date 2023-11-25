using App.Scripts.Libs.DataManager;
using App.Scripts.Libs.NodeArchitecture;

namespace App.Scripts.Scenes.PacksScene.Packs
{
    public class PacksContext : ContextNode
    {
        private DataManager _dataManager = new();
        
        protected override void OnConstruct()
        {
            RegisterInstance(_dataManager);
        }
    }
}