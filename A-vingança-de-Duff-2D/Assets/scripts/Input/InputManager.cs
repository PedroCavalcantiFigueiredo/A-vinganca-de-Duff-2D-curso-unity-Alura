using System;
using UnityEngine;
using UnityEngine.InputSystem; 

public class InputManager : MonoBehaviour
{
    private PlayerControls playerControls;
    // Variável renomeada para evitar conflito
    private bool _inputsEnabled = true; 

    // Propriedade agora verifica a variável com o novo nome
    public float Movement => _inputsEnabled ? playerControls.Gameplay.Movement.ReadValue<float>() : 0f;

    public event Action OnJump;
    public event Action OnAttack;
    
    public InputManager()
    {
        playerControls = new PlayerControls();
        playerControls.Gameplay.Enable();

        playerControls.Gameplay.Jump.performed += OnJumpPerformed;
        playerControls.Gameplay.Attack.performed += OnAttackPerformed;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        // Condição atualizada para usar a variável correta
        if (_inputsEnabled)
            OnJump?.Invoke();
    }

    private void OnAttackPerformed(InputAction.CallbackContext obj)
    {
        // Condição atualizada para usar a variável correta
        if (_inputsEnabled)
            OnAttack?.Invoke();
    }

    public void DisableInputs()
    {
        // Método agora modifica a variável correta
        _inputsEnabled = false;
        playerControls.Gameplay.Disable();
    }

    public void EnableInputs()
    {
        // Método agora modifica a variável correta
        _inputsEnabled = true;
        playerControls.Gameplay.Enable();
    }
}