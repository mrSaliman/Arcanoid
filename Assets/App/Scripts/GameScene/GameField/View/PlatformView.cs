using System;
using App.Scripts.Configs;
using App.Scripts.GameScene.Game;
using App.Scripts.Libs.ObjectPool;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.GameScene.GameField.View
{
    public class PlatformView : MonoBehaviour
    {
        [SerializeField] private PlatformSettings settings;
        [SerializeField] private PolygonCollider2D polygonCollider;
        [SerializeField] private Rigidbody2D platformRigidbody;

        private CameraInfoProvider _cameraInfoProvider;
        private Rect _cameraRect;

        private float _platformSize = 1;
        private float _colliderSize;

        [GameInject]
        public void Construct(CameraInfoProvider cameraInfoProvider)
        {
            _cameraInfoProvider = cameraInfoProvider;
            _cameraRect = _cameraInfoProvider.CameraRect;
            _colliderSize = polygonCollider.bounds.size.x;
        }

        public void MoveToTarget(float target)
        {
            target = Mathf.Clamp(target, _cameraRect.xMin + _colliderSize / 2, _cameraRect.xMax - _colliderSize / 2);
            var direction = new Vector2(target - platformRigidbody.position.x, settings.Smoothness).normalized;

            platformRigidbody.velocity = direction * settings.PlatformSpeed;
        }

        public void ChangeSize(float size)
        {
            _platformSize = Mathf.Clamp(size, settings.PlatformSize.x, settings.PlatformSize.y);

            var newScale = transform.localScale;
            newScale.x = _platformSize;
            transform.localScale = newScale;
        }
    }
}