using UnityEngine;
using UnityEngine.Events;
using System;

namespace PixelCrew.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private int _maxHealth;
        [SerializeField] private UnityEvent _OnDamage;
        [SerializeField] private UnityEvent _OnDie;
        [SerializeField] private UnityEvent _OnShieldBroke;
        [SerializeField] private HealthChangeEvent _OnChange;
        [SerializeField] private bool _isProtected;

        public void TakeShield()
        {
            _isProtected = true;
            Debug.Log("Shield is taken!");
        }
        public void ModifyHealth(int value)
        {
            if (value <= 0)
            {
                if (!_isProtected)
                {
                    _OnDamage?.Invoke();
                    _health += value;
                    _OnChange?.Invoke(_health);
                    if (_health <= 0)
                    {
                        _OnDie?.Invoke();
                    }
                }
                else
                {
                    _OnShieldBroke?.Invoke();
                    _isProtected = false;
                    Debug.Log("Shield is broken!");
                }
            } 
            else
            {
                _health += value;
                _OnChange?.Invoke(_health);
                if (_health > _maxHealth)
                {
                    _health = _maxHealth;
                }
                Debug.Log("Your health: " + _health);
            }    
        }
        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {

        }
        public void SetHealth(int health)
        {
            _health = health;
        }
    }
}

