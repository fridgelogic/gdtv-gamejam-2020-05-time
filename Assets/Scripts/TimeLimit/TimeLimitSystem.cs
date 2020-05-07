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
        private IntValue lowTimeLimit = null;

        [SerializeField]
        private GameEvent timeLimitUpdated = null;

        [SerializeField]
        private GameEvent lowTimeRemaining = null;

        [SerializeField]
        private GameEvent timeLimitExpired = null;

        private Coroutine runTimer;
        private bool firedTimeLimitLow;
        private float nextTimerTick;
        private float timerProgress;

        public void StartTimer()
        {
            nextTimerTick = Time.time + timeFactor - timerProgress;
            runTimer = StartCoroutine(RunTimer());
        }

        public void PauseTimer()
        {
            if (runTimer != null)
            {
                StopCoroutine(runTimer);
                runTimer = null;
            }
            timerProgress = timeFactor - (nextTimerTick - Time.time);
        }

        private IEnumerator RunTimer()
        {
            while (timeLimit.Value > 0)
            {
                yield return new WaitForSeconds(nextTimerTick - Time.time);
                timeLimit.Value--;
                timeLimitUpdated.Raise();

                if (!firedTimeLimitLow && timeLimit.Value <= lowTimeLimit.Value)
                {
                    firedTimeLimitLow = true;
                    lowTimeRemaining.Raise();
                }
                
                nextTimerTick = Time.time + timeFactor;
            }

            yield return new WaitForSeconds(1);
            timeLimitExpired.Raise();
        }
    }
}