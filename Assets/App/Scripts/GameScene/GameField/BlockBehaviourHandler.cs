using System;
using App.Scripts.GameScene.Game;
using App.Scripts.GameScene.GameField.Model;
using App.Scripts.GameScene.GameField.View;

namespace App.Scripts.GameScene.GameField
{
    public class BlockBehaviourHandler
    {
        private LevelView _levelView;
        private GameFieldManager _gameFieldManager;
        
        [GameInject]
        public void Construct(LevelView levelView, GameFieldManager manager)
        {
            _levelView = levelView;
            _gameFieldManager = manager;
        }
        
        public void HandleBlockDeath(BlockView blockView, Block block)
        {
            switch (block.blockType)
            {
                case BlockType.Simple:
                    break;
                case BlockType.Iron:
                    break;
                case BlockType.TNT:
                    break;
                case BlockType.VerticalTNT:
                    break;
                case BlockType.HorizontalTNT:
                    break;
                case BlockType.ColorTNT:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Delete(blockView);
        }

        private void Delete(BlockView blockView)
        {
            _gameFieldManager.RemoveBlock(blockView);
        }
    }
}