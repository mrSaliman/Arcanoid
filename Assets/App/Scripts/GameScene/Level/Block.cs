using System;
using App.Scripts.Libs.ObjectPool;

namespace App.Scripts.GameScene.Level
{
    [Serializable]
    public class Block : IPoolable
    {
        public int health;
        public BlockType blockType; 

        public event Action<Block> OnHealthDepleted;

        public Block(int initialHealth, int x, int y, BlockType type)
        {
            health = initialHealth;
            blockType = type;
        }

        public Block() { }

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                OnHealthDepleted?.Invoke(this);
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