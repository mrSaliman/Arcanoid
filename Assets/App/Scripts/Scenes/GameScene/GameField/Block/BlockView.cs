using System;
using App.Scripts.Libs.ObjectPool;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Scenes.GameScene.GameField.Block
{
    public class BlockView : MonoBehaviour, IPoolable
    {
        public Vector2Int gridPosition;
        public Action OnHealthDepletedHandler;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpriteRenderer crack;
        [SerializeField] private BoxCollider2D boxCollider;

        public Sprite Crack => crack.sprite;

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            boxCollider.size = sprite.bounds.size;
        }

        public void SetCrack(Sprite sprite)
        {
            if (!gameObject.activeSelf) return;
            crack.sprite = sprite;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            crack.sprite = null;
            OnHealthDepletedHandler = null;
            gameObject.SetActive(false);
        }
    }
}