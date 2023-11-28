using UnityEngine.UI;

namespace App.Scripts.Scenes.AllScenes.ProjectContext.Pop_Up
{
    public static class LayoutGroupExtensions
    {
        public static void Recalculate(this LayoutGroup lg)
        {
            lg.CalculateLayoutInputHorizontal();
            lg.SetLayoutHorizontal();
            lg.CalculateLayoutInputVertical();
            lg.SetLayoutVertical();
        }
    }
}