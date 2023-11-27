using System;
using System.Collections.Generic;
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
        [SerializeField] private SpriteRenderer crackPrefab;
        [SerializeField] private BoxCollider2D boxCollider;

        private List<SpriteRenderer> _mainCracks = new();

        public Sprite LastCrack => _mainCracks.Count >= 1 ? _mainCracks[^1].sprite : null;

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
            boxCollider.size = sprite.bounds.size;
        }

        public void AddCrack(Sprite sprite)
        {
            if (!gameObject.activeSelf) return;
            var crack = Instantiate(crackPrefab, transform);
            crack.sprite = sprite;
            _mainCracks.Add(crack);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            foreach (var crack in _mainCracks)
            {
                Destroy(crack);
            }
            _mainCracks.Clear();
            OnHealthDepletedHandler = null;
            gameObject.SetActive(false);
        }
    }
}