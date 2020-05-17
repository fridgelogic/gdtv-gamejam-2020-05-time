using System.Collections;
using FridgeLogic.ScriptableObjects.GameEvents;
using FridgeLogic.ScriptableObjects.Values;
using UnityEngine;

namespace FridgeLogic.TimeLimit
{
    public class TimeLimitSystem : MonoBehaviour
    {
        [SerializeField]
        private float timeFactor = 1f;

        [SerializeField]
        private IntValue timeLimit = null;

        [SerializeField]
        private GameEvent timeLimitUpdated = null;

        [SerializeField]
        private GameEvent timeLimitExpired = null;

        private Coroutine _runTimer;
        private bool _firedTimeLimitLow;
        private float _nextTimerTick;
        private float _timerProgress;

        public void StartTimer()
        {
            _nextTimerTick = Time.time + timeFactor - _timerProgress;
            _runTimer = StartCoroutine(RunTimer());
        }

        public void PauseTimer()
        {
            if (_runTimer != null)
            {
                StopCoroutine(_runTimer);
                _runTimer = null;
            }
            _timerProgress = timeFactor - (_nextTimerTick - Time.time);
        }

        public void ResetTimer()
        {
            PauseTimer();
            _timerProgress = 0f;
            timeLimit.Value = timeLimit.OriginalValue;
            StartTimer();
        }

        public void AddTime(IntValue value)
        {
            timeLimit.Value += value.Value;
            timeLimitUpdated.Raise();
        }

        private IEnumerator RunTimer()
        {
            while (timeLimit.Value > 0)
            {
                yield return new WaitForSeconds(_nextTimerTick - Time.time);
                timeLimit.Value--;
                timeLimitUpdated.Raise();
                _nextTimerTick = Time.time + timeFactor;
            }

            yield return new WaitForSeconds(1);
            timeLimitExpired.Raise();
        }

        private void Start()
        {
            _timerProgress = 0f;
            timeLimit.Value = timeLimit.OriginalValue;
        }
    }
}