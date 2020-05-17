using FridgeLogic.ScriptableObjects.Values;
using UnityEngine;
using UnityEngine.UI;

namespace FridgeLogic.UI
{
    public class TimeLimitPanel : MonoBehaviour
    {
        [SerializeField]
        private Text timeLimit = null;

        [SerializeField]
        private IntValue currentTimeLimit = null;

        [SerializeField]
        private Color lowTimeRemainingColor = Color.red;

        public void OnUpdateTimeLimit()
        {
            timeLimit.text = Mathf.Max(0, currentTimeLimit.Value).ToString();
        }

        public void OnLowTimeRemaining()
        {
            timeLimit.color = lowTimeRemainingColor;
        }

        private void Start()
        {
            OnUpdateTimeLimit();
        }
    }
}