using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Game
{
    [Serializable]
    public class GameFieldInfoProvider
    {
        [SerializeReference] public Camera mainCamera;
        [SerializeField] private RectTransform topperBg;
        [SerializeField] private CanvasScaler scaler;
        public Rect GameFieldRect
        {
            get
            {
                Vector2 position = mainCamera.transform.position;
                var width = mainCamera.orthographicSize * 2f * mainCamera.aspect; 
                var height = mainCamera.orthographicSize * 2f;
                
                var cameraRect = new Rect(position.x - width / 2, position.y - height / 2, width, height);
                cameraRect.yMax -= topperBg.rect.height / scaler.referencePixelsPerUnit;
                
                return cameraRect;
            }
        }
    }
}