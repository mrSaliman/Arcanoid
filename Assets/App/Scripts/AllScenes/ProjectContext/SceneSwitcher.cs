using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App.Scripts.AllScenes.ProjectContext
{
    public sealed class SceneSwitcher
    {
        public async UniTask LoadSceneAsync(string sceneName)
        {
            var async = SceneManager.LoadSceneAsync(sceneName);
            async.allowSceneActivation = false;

            while (async.progress < 0.89)
            {
                Debug.Log($"Progress: {async.progress}");
                await UniTask.Yield();
            }

            async.allowSceneActivation = true;
        }
    }
}