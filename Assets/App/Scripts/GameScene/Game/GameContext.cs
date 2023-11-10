using App.Scripts.Libs.DataManager;
using App.Scripts.Libs.NodeArchitecture;
using UnityEngine;

namespace App.Scripts.GameScene.Game
{
    public sealed class GameContext : ContextNode
    {
        private DataManager _dataManager = new();
        
        protected override void OnConstruct()
        {
            RegisterInstance(_dataManager);
        }
    }
}