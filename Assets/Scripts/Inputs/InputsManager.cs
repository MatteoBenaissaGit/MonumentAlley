using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputsManager
{
    public Action<Vector2> OnTouch { get; set; }
    
    private InputScheme _scheme;
    
    public void Initialize()
    {
        _scheme = new InputScheme();
        _scheme.Enable();
        
        _scheme.ActionMap.Touch.performed += OnTouchPerformed;
        _scheme.ActionMap.Click.performed += OnClickPerformed;
    }

    private void OnClickPerformed(InputAction.CallbackContext obj)
    {
        Vector2 position = Input.mousePosition;
        OnTouch?.Invoke(position);
    }

    private void OnTouchPerformed(InputAction.CallbackContext obj)
    {
        Vector2 position = obj.ReadValue<Vector2>();
        OnTouch?.Invoke(position);
    }
}
