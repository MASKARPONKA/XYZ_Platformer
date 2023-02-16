using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Hero _hero;
    public void OnMove(InputAction.CallbackContext context)
    {
        var dirVector = context.ReadValue<Vector2>();
        _hero.SetDirection(dirVector);
    }
    public void OnSayYohoho(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _hero.SayYohoho();
        }    
        
    }
}
