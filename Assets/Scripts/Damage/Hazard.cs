using UnityEngine;

namespace FridgeLogic.Damage
{
    [RequireComponent(typeof(Attacker))]
    public class Hazard : MonoBehaviour
    {
        private Attacker attacker = null;
        private Attacker Attacker => attacker ?? (attacker = GetComponent<Attacker>());

        private void OnTriggerEnter2D(Collider2D other)
        {
            var health = other.GetComponent<Health>();
            if (health)
            {
                Attacker.Attack(health);
            }
        }
    }
}
