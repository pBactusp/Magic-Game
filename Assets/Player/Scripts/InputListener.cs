using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MouseDelta;
    public Vector2 MoveComposite;
    public bool IsSprinting;

    public Action OnJumpPerformed;
    public Action OnStartedSprinting;
    public Action OnStoppedSprinting;

    public Action<int> OnCastingSpell;


    private Controls controls;

    public void OnEnable()
    {
        if (controls != null)
            return;

        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    public void OnDisable()
    {
        controls.Player.Disable();
    }



    public void OnLook(InputAction.CallbackContext context)
    {
        MouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveComposite = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnJumpPerformed?.Invoke();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsSprinting = true;
            OnStartedSprinting?.Invoke();
        }
        else if (context.canceled)
        {
            IsSprinting = false;
            OnStoppedSprinting?.Invoke();
        }
    }

    public void OnSpell1(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnCastingSpell?.Invoke(0);
    }

    public void OnSpell2(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnCastingSpell?.Invoke(1);
    }

    public void OnSpell3(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnCastingSpell?.Invoke(2);
    }

    public void OnSpell4(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnCastingSpell?.Invoke(3);
    }





}
