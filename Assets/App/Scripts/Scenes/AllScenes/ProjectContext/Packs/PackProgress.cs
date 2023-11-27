using System;

namespace App.Scripts.Scenes.AllScenes.ProjectContext.Packs
{
    [Serializable]
    public class PackProgress
    {
        public int progress;
        public int nextLevel;
        public bool discovered;
    }
}