using UnityEngine;

namespace FridgeLogic.Movement
{
    public class MovementCalculator
    {

        // https://en.wikipedia.org/wiki/Verlet_integration#Velocity_Verlet
        public static Vector2 CalculateTranslation(float gravity, ref Vector2 velocity)
        {
            var acceleration = new Vector2(0, gravity);
            var translation = velocity * Time.deltaTime + acceleration * Mathf.Pow(Time.deltaTime, 2) * 0.5f;
            velocity += acceleration * Time.deltaTime;
            return translation;
        }
    }
}