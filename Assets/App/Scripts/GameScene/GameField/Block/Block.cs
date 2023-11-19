using System;
using App.Scripts.Libs.ObjectPool;

namespace App.Scripts.GameScene.GameField.Block
{
    [Serializable]
    public class Block : IPoolable
    {
        public int health;
        public BlockType blockType;

        public event Action OnHealthDepleted;

        public void TakeDamage(int damage)
        {
            if (health <= 0) return;
            health -= damage;
            if (health <= 0)
            {
                OnHealthDepleted?.Invoke();
            }
        }

        public void Copy(Block block)
        {
            health = block.health;
            blockType = block.blockType;
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }
    }
}