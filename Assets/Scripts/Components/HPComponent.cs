using UnityEngine;

namespace PixelCrew.Components
{
    public class HPComponent : MonoBehaviour
    {
        [SerializeField] private int _value;
        
        public void Modify(GameObject target)
        {
            var healtComponent = target.GetComponent<HealthComponent>();
            if (healtComponent != null)
            {
                healtComponent.ApplyDamage(_value);
            }
        }
    }
}