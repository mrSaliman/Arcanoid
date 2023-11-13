using App.Scripts.GameScene.Game;
using App.Scripts.Libs.NodeArchitecture;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.GameScene.Level
{
    public class GameFieldContext : ContextNode
    {
        [SerializeField] private GameFieldManager gameFieldManager;
        private LevelLoader _levelLoader = new();
        
        protected override void OnConstruct()
        {
            RegisterInstance(gameFieldManager);
            RegisterInstance(_levelLoader);
        }
    }
}