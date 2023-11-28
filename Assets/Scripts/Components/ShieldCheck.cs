using UnityEngine;

namespace PixelCrew.Components
{
    public class ShieldCheck : MonoBehaviour
    {
        private Hero _hero;

        private void Start()
        {
            _hero = FindObjectOfType<Hero>();
        }
        public void Protect()
        {
            var HP = _hero.GetComponent<HealthComponent>();
            if (HP != null)
            {
                HP.TakeShield();
            }
        }
    }
}