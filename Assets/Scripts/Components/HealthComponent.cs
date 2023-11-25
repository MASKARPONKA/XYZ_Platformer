using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private int _maxHealth;
        [SerializeField] private UnityEvent _OnDamage;
        [SerializeField] private UnityEvent _OnDie;

        public void ApplyDamage(int value)
        {
            if(value <= 0) _OnDamage?.Invoke();
            _health += value;
            if (_health <= 0)
            {
                _OnDie?.Invoke();
            }
            else if (_health > _maxHealth)
            {
                _health = _maxHealth;
            }
            Debug.Log("Your health: " + _health);
        }
    }
}

