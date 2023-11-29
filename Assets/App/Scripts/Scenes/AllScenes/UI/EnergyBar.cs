using App.Scripts.Scenes.AllScenes.ProjectContext.Energy;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace App.Scripts.Scenes.AllScenes.UI
{
    public class EnergyBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI timerLabel;
        [SerializeField] private TextMeshProUGUI amountLabel;

        public int maxEnergy = 100;

        private Tweener _currentTweener;
        private int _currentAmount = -1;

        public void SetValues(EnergyInfo info)
        {
            if (_currentAmount == info.Amount) return;
            if (_currentTweener is not null && _currentTweener.active) _currentTweener.Kill();
            _currentTweener = DOVirtual.Float(slider.value, info.Amount + 0.1f, 1, OnVirtualUpdate);
            timerLabel.text = info.NextGainTime.Hours > 0
                ? info.NextGainTime.ToString(@"hh\:mm\:ss")
                : info.NextGainTime.ToString(@"mm\:ss");
        }

        private void OnVirtualUpdate(float value)
        {
            slider.value = value;
            amountLabel.text = $"{(int)value}/{maxEnergy}";
        }
    }
}