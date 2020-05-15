using UnityEngine;

namespace FridgeLogic.Movement
{
    [CreateAssetMenu(fileName = "MovementOptions", menuName = "FridgeLogic/Movement/Options", order = 53)]
    public class MovementOptions : ScriptableObject
    {
        public float groundSpeed = 12f;
        public float runSpeedModifier = 1.75f;
        public float jumpHeight = 4f;
        public float timeToJumpApex = 0.4f;
        public float accelerationTimeInAir = 0.25f;
        public float accelerationTimeWalking = 0.1f;
        public float accelerationTimeRunning = 0.2f;
        public float jumpBufferTime = 0.1f;
        public float coyoteTime = 0.1f;
        public float stopJumpRate = 0.5f;
    }
}