using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private Vector3 _direction;
    [SerializeField] float _speed;
   public void SetDirection(Vector2 directionVector)
    {
        _direction = directionVector;
    }
    public void SayYohoho()
    {
        Debug.Log("Yohoho!");
    }
    private void Update()
    {
        if (_direction != Vector3.zero)
        {
            var delta = _direction * _speed * Time.deltaTime;
            transform.position += delta;
        }
    }
}
