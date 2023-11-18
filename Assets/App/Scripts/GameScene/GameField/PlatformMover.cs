using System;
using App.Scripts.GameScene.Game;
using App.Scripts.GameScene.GameField.View;
using App.Scripts.Libs.NodeArchitecture;
using UnityEngine;

namespace App.Scripts.GameScene.GameField
{
    public class PlatformMover : IContextFixedUpdate
    {
        private MouseInput _mouse;
        private PlatformView _platform;

        [GameInject]
        public void Construct(MouseInput mouseInput, PlatformView platformView)
        {
            _mouse = mouseInput;
            _platform = platformView;
        }
        
        public void OnFixedUpdate()
        {
            switch (_mouse.LeftButton)
            {
                case MouseInput.ButtonState.Down:
                case MouseInput.ButtonState.Press:
                    _platform.MoveToTarget(_mouse.Position.x);
                    break;
            }
        }
    }
}