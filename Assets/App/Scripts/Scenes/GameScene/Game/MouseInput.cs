using App.Scripts.Libs.NodeArchitecture;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Game
{
    public class MouseInput : IContextUpdate
    {
        public enum ButtonState
        {
            Up,
            Down,
            Press,
            Free
        }

        public ButtonState LeftButton { get; private set; }
        public Vector3 Position { get; private set; }

        private Rect _mouseInputZone;
        private GameFieldInfoProvider _gameFieldInfoProvider;
        private GameManager _gameManager;
        

        [GameInject]
        public void Construct(GameFieldInfoProvider gameFieldInfoProvider, GameManager gameManager)
        {
            _gameFieldInfoProvider = gameFieldInfoProvider;
            _mouseInputZone = _gameFieldInfoProvider.GameFieldRect;
            _gameManager = gameManager;
        }

        [GamePause]
        public void Pause()
        {
            LeftButton = ButtonState.Free;
        }
        
        [GameFinish]
        public void Finish()
        {
            LeftButton = ButtonState.Free;
        }
        
        public void OnUpdate()
        {
            if (_gameManager.GameState != GameState.Playing) return;
            Position = _gameFieldInfoProvider.mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (_mouseInputZone.Contains(Position))
            {
                if (Input.GetMouseButton(0))
                {
                    LeftButton = ButtonState.Press;
                    if (Input.GetMouseButtonDown(0)) LeftButton = ButtonState.Down;
                }
                else
                {
                    LeftButton = ButtonState.Free;
                    if (Input.GetMouseButtonUp(0)) LeftButton = ButtonState.Up;
                }
            }
            else
            {
                LeftButton = ButtonState.Free;
            }
        }
    }
}