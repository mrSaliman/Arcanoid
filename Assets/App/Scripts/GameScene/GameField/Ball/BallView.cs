using System;
using App.Scripts.Libs.ObjectPool;
using UnityEngine;

namespace App.Scripts.GameScene.GameField.Ball
{
    public class BallView : MonoBehaviour, IPoolable
    {
        [SerializeField] private Rigidbody2D mainRigidbody;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private CircleCollider2D mainCollider;
        
        public Rigidbody2D MainRigidbody => mainRigidbody;
        public SpriteRenderer SpriteRenderer => spriteRenderer;

        public event Action<BallView, Collision2D> OnCollisionEnterEvent;
        public event Action<BallView, Collision2D> OnCollisionExitEvent;

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }
        
        public void SetVelocity(Vector2 velocity)
        {
            mainRigidbody.velocity = velocity;
        }

        public void SetSpeed(float speed)
        {
            mainRigidbody.velocity = mainRigidbody.velocity.normalized * speed;
        }

        public void SetSimulated(bool simulated)
        {
            mainRigidbody.simulated = simulated;
            mainCollider.enabled = simulated;
        }

        public void Show()
        {
            spriteRenderer.enabled = true;
        }

        public void Hide()
        {
            spriteRenderer.enabled = false;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            OnCollisionEnterEvent?.Invoke(this, other);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            OnCollisionExitEvent?.Invoke(this, other);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            Hide();
            OnCollisionEnterEvent = null;
            OnCollisionExitEvent = null;
            SetVelocity(Vector2.zero);
            SetSimulated(false);
            gameObject.transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}