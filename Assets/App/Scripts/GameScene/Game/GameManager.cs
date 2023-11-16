using App.Scripts.AllScenes.ProjectContext;
using App.Scripts.Libs.JsonResourceLoader;
using App.Scripts.Libs.NodeArchitecture;
using Unity.VisualScripting;
using UnityEngine;

namespace App.Scripts.GameScene.Game
{
    [RequireComponent(typeof(GameContext))]
    public class GameManager : MonoBehaviour
    {
        public GameState GameState { get; private set; }

        [SerializeField]
        private GameContext context;
        private ContextNode _root;
        
        [SerializeField]
        private bool autoRun = true;

        public Rigidbody2D rb;
         
        private void Awake()
        {
            context.RegisterInstance(this);
            _root = ProjectContext.Instance;
            _root.Construct();
            _root.AddChild(context);
            
            GameState = GameState.Off;

            if (autoRun)
            {
                ConstructGame();
                InitGame();
                StartGame();
                rb.velocity = new Vector2(5, -5);
            }
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

        [ContextMenu("Start")]
        public void StartGame()
        {
            _root.SendEvent<GameStart>();
            GameState = GameState.Playing;
        }

        [ContextMenu("Pause")]
        public void PauseGame()
        {
            _root.SendEvent<GamePause>();
            GameState = GameState.Paused;
        }

        [ContextMenu("Resume")]
        public void ResumeGame()
        {
            _root.SendEvent<GameResume>();
            GameState = GameState.Playing;
        }

        [ContextMenu("Finish")]
        public void FinishGame()
        {
            _root.SendEvent<GameFinish>();
            GameState = GameState.Finished;
        }
    }
}