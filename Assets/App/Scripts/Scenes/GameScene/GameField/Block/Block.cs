using System;
using App.Scripts.Libs.ObjectPool;

namespace App.Scripts.Scenes.GameScene.GameField.Block
{
    [Serializable]
    public class Block : IPoolable
    {
        public int health;
        public BlockType blockType;

        public event Action OnHealthDepleted;

        public bool TakeDamage(int damage)
        {
            if (health <= 0) return false;
            health -= damage;
            if (health > 0) return true;
            OnHealthDepleted?.Invoke();
            return false;

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
            OnHealthDepleted = null;
        }
    }
}