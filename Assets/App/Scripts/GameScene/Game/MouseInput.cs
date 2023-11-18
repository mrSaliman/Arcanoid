using App.Scripts.Libs.NodeArchitecture;
using UnityEngine;

namespace App.Scripts.GameScene.Game
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
        private CameraInfoProvider _cameraInfoProvider;
        

        [GameInject]
        public void Construct(CameraInfoProvider cameraInfoProvider)
        {
            _cameraInfoProvider = cameraInfoProvider;
            _mouseInputZone = _cameraInfoProvider.CameraRect;
            _mouseInputZone.yMax -= 0.1f * _mouseInputZone.height;
        }
        
        public void OnUpdate()
        {
            Position = _cameraInfoProvider.mainCamera.ScreenToWorldPoint(Input.mousePosition);
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