namespace App.Scripts.Libs.Architecture.Scenes
{
    public sealed class SceneManagerExample : SceneManagerBase
    {
        public override void InitScenesMap()
        {
            SceneConfigMap[SceneConfigExample.SCENE_NAME] = new SceneConfigExample();
        }
    }
}