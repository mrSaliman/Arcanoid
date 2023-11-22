using System.Collections.Generic;
using App.Scripts.AllScenes.ProjectContext;
using App.Scripts.AllScenes.ProjectContext.Pop_Up;
using App.Scripts.AllScenes.UI;
using App.Scripts.GameScene.Game;
using App.Scripts.Libs.NodeArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.MainMenu.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        private MenuContext context;
        private ContextNode _root;

        [SerializeField]
        private UIContext uiContext;
        private LabelController _labelController;
        private PopupManager _popupManager;
        private Popup _currentPopup;
        [SerializeField] private Transform canvas;
        
        private void Awake()
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

        [GameInject]
        public void Construct(PopupManager popupManager)
        {
            _popupManager = popupManager;
            _labelController = uiContext.ResolveInstance<LabelController>();
        }


        private List<Popup> _popups = new();
        
        [Button]
        private void AddPopup()
        {
            var builder = new PopupBuilder(_popupManager);
            builder.Fit(true);
            builder.vertical = false;
            builder.AddLabel(_labelController, "continue", 60, Color.green)
                .AddButton(_labelController, "pause", 40, Color.yellow, () => Debug.Log("pause clicked"))
                .AddButton(_labelController, "play", 40, Color.red, () => Debug.Log("play clicked"))
                .AddButton(_labelController, "stop", 40, Color.magenta, () => Debug.Log("stop clicked"));
            var popup = builder.Build();
            popup.transform.SetParent(canvas, false);
            popup.gameObject.SetActive(true);
            _popups.Add(popup);
        }

        [Button]
        private void RemoveFirstPopup()
        {
            _popupManager.Return(_popups[0]);
            _popups.RemoveAt(0);
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