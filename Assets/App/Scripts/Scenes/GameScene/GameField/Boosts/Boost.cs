using System;
using App.Scripts.Libs.ObjectPool;
using App.Scripts.Scenes.GameScene.GameField.Platform;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Scenes.GameScene.GameField.Boosts
{
    public class Boost : MonoBehaviour, IPoolable
    {
        public Action OnBoostApplied;
        public SpriteRenderer spriteRenderer;
        public Rigidbody2D rb;

        private void ApplyBoost()
        {
            OnBoostApplied?.Invoke();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlatformView _))
            {
                ApplyBoost();
            }
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
            OnBoostApplied = null;
            rb.velocity = Vector2.zero;
            rb.simulated = true;
            gameObject.SetActive(false);
        }
    }

}