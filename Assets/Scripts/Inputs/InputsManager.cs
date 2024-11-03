using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputsManager
{
    public Action<Vector2> OnTouch { get; set; }
    public Action<Vector2> OnRelease { get; set; }
    
    private InputScheme _scheme;
    private bool _isPressing;
    private Vector2 _pressingPosition;
    
    public void Initialize()
    {
        _scheme = new InputScheme();
        _scheme.Enable();
        
        _scheme.ActionMap.Touch.performed += OnTouchPerformed;
        _scheme.ActionMap.Click.performed += OnClickPerformed;
        
        _scheme.ActionMap.Touch.canceled += OnReleasePerformed;
        _scheme.ActionMap.Click.canceled += OnReleasePerformed;
    }

    private void OnClickPerformed(InputAction.CallbackContext obj)
    {
        Vector2 position = Input.mousePosition;
        OnTouch?.Invoke(position);
        _isPressing = true;
    }

    private void OnTouchPerformed(InputAction.CallbackContext obj)
    {
        Vector2 position = obj.ReadValue<Vector2>();
        OnTouch?.Invoke(position);
        _isPressing = true;
    }
    
    private void OnReleasePerformed(InputAction.CallbackContext obj)
    {
        Vector2 position = obj.ReadValue<Vector2>();
        OnRelease?.Invoke(position);
        _isPressing = false;
    }

    public bool IsPressingPosition(out Vector2 position)
    {
        _pressingPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y); 
        
        position = _pressingPosition;
        if (_isPressing == false)
        {
            return false;
        }
        
        return true;
    }
}
