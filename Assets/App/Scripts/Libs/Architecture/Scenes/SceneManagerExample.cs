namespace App.Scripts.Libs.Architecture.Scenes
{
    public class SceneManagerExample : SceneManagerBase
    {
        protected override void InitScenesMap()
        {
            _sceneConfigMap[SceneConfigExample.SCENE_NAME] = new SceneConfigExample();
        }
    }
}