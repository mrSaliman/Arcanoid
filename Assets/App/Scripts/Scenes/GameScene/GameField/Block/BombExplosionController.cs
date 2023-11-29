using System;
using System.Collections.Generic;
using App.Scripts.Configs;
using App.Scripts.Scenes.GameScene.Game;
using App.Scripts.Scenes.GameScene.GameField.Level;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Scripts.Scenes.GameScene.GameField.Block
{
    [Serializable]
    public class BombExplosionController
    {
        [SerializeField] private BombsExplosionSettings settings;
        [SerializeField] private Transform effectsFolder;

        private GameFieldManager _gameFieldManager;
        private LevelView _levelView;

        [GameInject]
        public void Construct(GameFieldManager gameFieldManager, LevelView levelView)
        {
            _gameFieldManager = gameFieldManager;
            _levelView = levelView;
        }
        
        public void Explode(BlockView blockView, Block block)
        {
            var behaviour = settings.BombBehaviours[block.blockType];
            if (behaviour.isChainBomb) ExplodeChainBomb(blockView.gridPosition, behaviour.moves, behaviour.damage);
            else ExplodeBomb(blockView.gridPosition, behaviour.moves, behaviour.damage);
        }

        private void ExplodeBomb(Vector2Int startPosition, List<Vector3Int> moves, int damage)
        {
            var level = _gameFieldManager.CurrentLevel;
            var startElement = new ChainElement
            {
                NextElements = new List<ChainElement>(),
                Position = startPosition,
                Epoch = 0
            };

            foreach (var move in moves)
            {
                var currentElement = startElement;
                Vector2Int moveVector = new(move.x, move.y);
                var moveCount = move.z;
                while (moveCount != 0)
                {
                    moveCount--;
                    var nextPosition = currentElement.Position + moveVector;
                    if (level.Fits(nextPosition.x, nextPosition.y))
                    {
                        var nextElement = new ChainElement
                        {
                            NextElements = new List<ChainElement>(),
                            Position = nextPosition,
                            Epoch = currentElement.Epoch + 1
                        };
                        currentElement.NextElements.Add(nextElement);
                        currentElement = nextElement;
                        continue;
                    }

                    break;
                }
            }

            ExplodeSubsequence(startElement, false, damage, level).Forget();
        }

        private void ExplodeChainBomb(Vector2Int startPosition, List<Vector3Int> moves, int damage)
        {
            var level = _gameFieldManager.CurrentLevel;
            var startElement = new ChainElement
            {
                NextElements = new List<ChainElement>(),
                Position = startPosition,
                Epoch = 0
            };

            var maxLength = 0;
            var chainsCount = new Dictionary<Sprite, int>();
            var startPoint = startElement;
            foreach (var move in moves)
            {
                Vector2Int moveVector = new(move.x, move.y);
                var nextPosition = startElement.Position + moveVector;
                
                if (!level.Fits(nextPosition.x, nextPosition.y)) continue;
                
                var currentSprite = level.GetSprite(nextPosition.x, nextPosition.y);
                if (currentSprite == null || chainsCount.ContainsKey(currentSprite)) continue;
                
                var field = new bool[level.Width, level.Height];
                field[startPosition.x, startPosition.y] = true;
                
                var altStartElement = new ChainElement
                {
                    NextElements = new List<ChainElement>(),
                    Position = startPosition,
                    Epoch = 0
                };
                chainsCount[currentSprite] = 0;
                
                var queue = new Queue<ChainElement>();
                queue.Enqueue(altStartElement);
                
                while (queue.TryDequeue(out var element))
                {
                    foreach (var i in moves)
                    {
                        moveVector = new Vector2Int(i.x, i.y);
                        nextPosition = element.Position + moveVector;
                        
                        if (!level.Fits(nextPosition.x, nextPosition.y)) continue;
                        var nextSprite = level.GetSprite(nextPosition.x, nextPosition.y);
                        
                        if (currentSprite != nextSprite || field[nextPosition.x, nextPosition.y]) continue;
                        
                        field[nextPosition.x, nextPosition.y] = true;
                        var currentElement = new ChainElement
                        {
                            NextElements = new List<ChainElement>(),
                            Position = nextPosition,
                            Epoch = element.Epoch + 1
                        };
                                
                        element.NextElements.Add(currentElement);
                        queue.Enqueue(currentElement);
                        chainsCount[currentSprite]++;
                    }
                }

                if (chainsCount[currentSprite] <= maxLength) continue;
                maxLength = chainsCount[currentSprite];
                startPoint = altStartElement;
            }
            ExplodeSubsequence(startPoint, true, damage, level).Forget();
        }

        private async UniTaskVoid ExplodeSubsequence(ChainElement startElement, bool isStoppable, int damage,
            Level.Level level)
        {
            var currentElement = startElement;
            var epoch = 0;
            var queue = new Queue<ChainElement>();
            foreach (var element in startElement.NextElements)
            {
                queue.Enqueue(element);
            }

            while (queue.TryDequeue(out var element))
            {
                while (epoch < element.Epoch)
                {
                    await UniTask.WaitForSeconds(settings.ExplosionDelay);
                    epoch++;
                }

                var block = level.GetBlock(element.Position.x, element.Position.y);
                if (block != null)
                {
                    block.TakeDamage(damage);
                }
                else
                {
                    if (isStoppable) element.NextElements.Clear(); 
                }

                foreach (var nextElement in element.NextElements)
                {
                    queue.Enqueue(nextElement);
                }
                
                AnimateExplode(element.Position);
            }
        }

        private void AnimateExplode(Vector2Int position)
        {
            Object.Instantiate(settings.ExplosionEffect,  _levelView.GetBlockPosition(position),
                Quaternion.identity, effectsFolder);
        }

        public class ChainElement
        {
            public int Epoch;
            public Vector2Int Position;
            public List<ChainElement> NextElements;
        }
    }
}