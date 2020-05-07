using System;
using UnityEngine;

namespace FridgeLogic.ScriptableObjects.Values
{
    [CreateAssetMenu(fileName = "IntValue", menuName = "FridgeLogic/Values/IntValue", order = 53)]
    public class IntValue : ScriptableObject
    {
        [SerializeField]
        private int defaultValue = 0;

        [NonSerialized]
        private int delta = 0;

        public int Value
        {
            get => defaultValue + delta;
            set => delta = value - defaultValue;
        }
    }
}