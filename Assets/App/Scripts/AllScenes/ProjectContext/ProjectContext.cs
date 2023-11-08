using App.Scripts.GameScene.Game;
using App.Scripts.Libs.NodeArchitecture;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.AllScenes.ProjectContext
{
    public sealed class ProjectContext : ContextNode
    {
        private static ProjectContext _instance;

        public static ProjectContext Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                _instance = FindObjectOfType<ProjectContext>();
                if (_instance != null) return _instance;
                
                
                _instance = Resources.Load<ProjectContext>("ProjectContext");

                if (_instance != null)
                {
                    _instance = Instantiate(_instance);
                    _instance.name = "ProjectContext";
                    _instance.RegisterMainInstances();

                    DontDestroyOnLoad(_instance);
                }
                else
                {
                    Debug.LogError("Prefab not found or not set in Resources.");
                }

                return _instance;
            }
        }

        private readonly SceneSwitcher _sceneSwitcher = new();
        [SerializeField] private LocalizationManager localizationManager = new();

        private void RegisterMainInstances()
        {
            RegisterInstance(_sceneSwitcher);
            RegisterInstance(localizationManager);
        }
    }
}