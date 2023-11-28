using System;
using System.Collections.Generic;
using System.IO;
using App.Scripts.Configs;
using App.Scripts.Libs.JsonDataService;
using App.Scripts.Scenes.GameScene.Game;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.AllScenes.ProjectContext.Packs
{
    [Serializable]
    public class PacksController
    {
        private SceneSwitcher _sceneSwitcher;
        
        [SerializeField] private string savePath;
        [SerializeField] private LevelPacks packs;
        private PackResults _packResults;
        
        [FilePath(ParentFolder = "Assets/App/Resources", Extensions = "json", IncludeFileExtension = false)]
        public string levelToRun;
        
        public LevelPacks Packs => packs;
        public PackResults PackResults => _packResults;
        public int StartedLevel { get; private set; } = -1;

        [GameInject]
        public void Construct(SceneSwitcher sceneSwitcher)
        {
            _sceneSwitcher = sceneSwitcher;
        }

        [GameInit]
        public void Init()
        {
            if (_packResults != null) return;   
            if (JsonDataService.TryLoadData(savePath, out _packResults))
            {
                if (packs.Packs.Count != _packResults.packs.Count)
                {
                    Debug.LogError("Packs size discrepancy.");
                    throw new Exception();
                }

                for (var i = 0; i < _packResults.packs.Count; i++)
                {
                    var packResult = _packResults.packs[i];
                    var pack = packs.Packs[i];
                    if ((packResult.discovered || (packResult.progress == 0 && packResult.nextLevel == 0)) &&
                        packResult.progress >= packResult.nextLevel && packResult.progress <= pack.levels.Count &&
                        packResult.nextLevel < pack.levels.Count) continue;
                    Debug.LogError("Saved data is broken");
                    throw new Exception();
                }
            }
            else
            {
                _packResults = new PackResults();
                if (packs.Packs.Count < 1)
                {
                    Debug.LogError("Packs are empty!");
                    throw new Exception();
                }

                _packResults.packs = new List<PackProgress>();

                for (var i = 0; i < packs.Packs.Count; i++)
                {
                    _packResults.packs.Add(new PackProgress
                    {
                        discovered = false,
                        nextLevel = 0,
                        progress = 0
                    });
                }

                _packResults.packs[0].discovered = true;
            }
        }

        public void StartLevel(int packNumber)
        {
            if (packNumber >= _packResults.packs.Count)
            {
                Debug.LogError("Pack index out of range!");
                return;
            }

            if (!_packResults.packs[packNumber].discovered)
            {
                Debug.LogError("You can't start not discovered pack!");
                return;
            }

            StartedLevel = packNumber;
            levelToRun = packs.Packs[packNumber].levels[_packResults.packs[packNumber].nextLevel].path;
            _sceneSwitcher.LoadSceneAsync("GameScene").Forget();
        }

        public void SaveLevelResult(LevelResult result)
        {
            if (StartedLevel == -1 || result != LevelResult.Win) return;
            var resultsPack = _packResults.packs[StartedLevel];
            var pack = packs.Packs[StartedLevel];
            if (resultsPack.nextLevel == resultsPack.progress && resultsPack.progress < pack.levels.Count)
            {
                resultsPack.progress++;
                if (resultsPack.progress == pack.levels.Count && _packResults.packs.Count > StartedLevel + 1)
                    _packResults.packs[StartedLevel + 1].discovered = true;
            }
            resultsPack.nextLevel++;
            resultsPack.nextLevel %= pack.levels.Count;
            JsonDataService.SaveData(savePath, _packResults);
        }

        [Button]
        private void ResetSaves()
        {
            var path = Path.Combine(Application.persistentDataPath , savePath);
            if (File.Exists(path)) File.Delete(path);
        }
    }

    public enum LevelResult
    {
        Win,
        Lose
    }
}