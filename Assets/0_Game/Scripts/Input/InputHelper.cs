using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHelper : MonoBehaviour, InputProvider.IMouseActions
{
    public static InputHelper Instance;

    private InputProvider _inputProvider;

    public Vector2 MouseDelta { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        _inputProvider = new InputProvider();
        _inputProvider.Mouse.SetCallbacks(this);
        _inputProvider.Enable();

    }

    public void OnMouseDelta(InputAction.CallbackContext context)
    {
        MouseDelta = context.ReadValue<Vector2>();
    }
}


