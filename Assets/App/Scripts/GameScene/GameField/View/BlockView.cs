using System;
using App.Scripts.Libs.ObjectPool;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.GameScene.GameField.View
{
    public class BlockView : MonoBehaviour, IPoolable
    {
        public Vector2Int gridPosition;
        public Action OnHealthDepletedHandler;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BoxCollider2D boxCollider;

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            boxCollider.size = sprite.bounds.size;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            OnHealthDepletedHandler = null;
            gameObject.SetActive(false);
        }
    }
}