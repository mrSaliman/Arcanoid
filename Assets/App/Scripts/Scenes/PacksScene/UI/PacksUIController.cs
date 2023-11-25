using App.Scripts.Scenes.AllScenes.UI;
using App.Scripts.Scenes.GameScene.Game;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.PacksScene.UI
{
    public class PacksUIController : MonoBehaviour
    {
        [SerializeField] private RectTransform packsContainer;
        [SerializeField] private Button packButtonPrefab;

        private LabelController _labelController;

        [GameInject]
        public void Construct(LabelController labelController)
        {
            _labelController = labelController;
        }

        [GameInit]
        public void Init()
        {
        }
    }
}