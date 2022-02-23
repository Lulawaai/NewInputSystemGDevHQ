using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInputManager : MonoBehaviour
{
	private GameInputActions _input;

	public static event Action<Vector2> OnPlayerMove;
	public static event Action OnInteractableKeyE;
	public static event Action OnHoldKey;
	public static event Action OnHoldFinished;

	private 

	void Start()
	{
		_input = new GameInputActions();
		_input.Enable();

		_input.General.Ekey.performed += InteractableArea_performed;
		_input.General.Ekey.started += InteractableArea_started;
		_input.General.Ekey.canceled += InteractableArea_canceled;
	}

	#region //Input Events
	private void InteractableArea_started(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		OnHoldKey?.Invoke();
	}

	private void InteractableArea_canceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		OnHoldFinished?.Invoke();
	}

	private void InteractableArea_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		OnInteractableKeyE?.Invoke();
	}
	#endregion

	void Update()
    {
		PlayerMove();
    }

	private void PlayerMove()
	{
		Vector2 move;
		move = _input.PlayerInput.Move.ReadValue<Vector2>();

		OnPlayerMove?.Invoke(move);
	}
}
