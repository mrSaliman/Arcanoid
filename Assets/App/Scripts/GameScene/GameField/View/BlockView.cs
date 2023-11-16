using App.Scripts.Libs.ObjectPool;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.GameScene.GameField.View
{
    public class BlockView : MonoBehaviour, IPoolable
    {
        public Vector2 gridPosition;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BoxCollider2D boxCollider;

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            boxCollider.size = sprite.bounds.size;
        }

        public void SelfDestroy()
        {
            
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}