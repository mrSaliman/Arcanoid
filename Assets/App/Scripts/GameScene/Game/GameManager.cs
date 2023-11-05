using App.Scripts.AllScenes.ProjectContext;
using UnityEngine;

namespace App.Scripts.GameScene.Game
{
    [RequireComponent(typeof(GameContext))]
    public class GameManager : MonoBehaviour
    {
        public GameState GameState { get; private set; }

        [SerializeField]
        private GameContext context;
        
        [SerializeField]
        private bool autoRun = true;
        
        private void Awake()
        {
            context.RegisterInstance(this);
            var prContext = ProjectContext.Instance;
            prContext.Construct();
            prContext.AddChild(context);
            
            GameState = GameState.Off;

            if (autoRun)
            {
                ConstructGame();
                StartGame();
            }
        }

        private void Update()
        {
            context.OnUpdate();
        }

        private void FixedUpdate()
        {
            context.OnFixedUpdate();
        }

        private void LateUpdate()
        {
            context.OnLateUpdate();
        }
        
        [ContextMenu("Inject")]
        public void ConstructGame()
        {
            context.SendEvent<GameInject>();
        }

        [ContextMenu("Start")]
        public void StartGame()
        {
            context.SendEvent<GameStart>();
            GameState = GameState.Playing;
        }

        [ContextMenu("Pause")]
        public void PauseGame()
        {
            context.SendEvent<GamePause>();
            GameState = GameState.Paused;
        }

        [ContextMenu("Resume")]
        public void ResumeGame()
        {
            context.SendEvent<GameResume>();
            GameState = GameState.Playing;
        }

        [ContextMenu("Finish")]
        public void FinishGame()
        {
            context.SendEvent<GameFinish>();
            GameState = GameState.Finished;
        }
    }
}