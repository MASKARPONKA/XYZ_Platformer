using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private float _direction;
    [SerializeField] float _speed;
   public void SetDirection(float direction)
    {
        _direction = direction;
    }
    private void Update()
    {
        if (_direction != 0)
        {
            var delta = _direction * _speed * Time.deltaTime;
            var NewXPosition = transform.position.x + delta;
            transform.position = new Vector3(NewXPosition, transform.position.y, transform.position.z);
        }
    }
}
