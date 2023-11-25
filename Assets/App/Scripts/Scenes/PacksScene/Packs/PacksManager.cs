using App.Scripts.Libs.NodeArchitecture;
using App.Scripts.Scenes.AllScenes.ProjectContext;
using App.Scripts.Scenes.GameScene.Game;
using UnityEngine;

namespace App.Scripts.Scenes.PacksScene.Packs
{
    public class PacksManager : MonoBehaviour
    {
        [SerializeField]
        private PacksContext context;
        private ContextNode _root;
        
        private void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            
            context.RegisterInstance(this);
            _root = ProjectContext.Instance;
            _root.Construct();
            _root.AddChild(context);
            
            ConstructGame();
            InitGame();
        }

        public void Secede()
        {
            _root.RemoveChild(context);
        }
        
        private void Update()
        {
            _root.OnUpdate();
        }

        private void FixedUpdate()
        {
            _root.OnFixedUpdate();
        }

        private void LateUpdate()
        {
            _root.OnLateUpdate();
        }
        
        [ContextMenu("Inject")]
        public void ConstructGame()
        {
            _root.SendEvent<GameInject>();
        }

        [ContextMenu("Init")]
        public void InitGame()
        {
            _root.SendEvent<GameInit>();
        }
    }
}