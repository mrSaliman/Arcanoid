using App.Scripts.Libs.DataManager;
using App.Scripts.Libs.NodeArchitecture;

namespace App.Scripts.Scenes.MainMenu.Menu
{
    public class MenuContext : ContextNode
    {
        private DataManager _dataManager = new();
        
        protected override void OnConstruct()
        {
            RegisterInstance(_dataManager);
        }
    }
}