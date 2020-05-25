using FridgeLogic.Core;
using UnityEngine;
using UnityEngine.UI;

namespace FridgeLogic.UI
{
    public class TimeLimitPanel : MonoBehaviour
    {
        [SerializeField] private Text _timeLimit = null;
        [SerializeField] private TimeManager _timeManager = null;
        
        private int _lastTimeValue;

        private void UpdateTimer()
        {
            var remainingTime = Mathf.CeilToInt(_timeManager.RemainingTime);
            if (remainingTime != _lastTimeValue)
            {
                _lastTimeValue = remainingTime;
                _timeLimit.text = remainingTime.ToString("D2");
            }
        }

        private void Update()
        {
            UpdateTimer();
        }
    }
}