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
	public static event Action<float> OnDroneRotation;
	public static event Action OnSpaceKeyDronePressed;
	public static event Action OnVKeyDronePressed;
	public static event Action<Vector2> OnTiltDrone;

	void Start()
	{
		_input = new GameInputActions();
		_input.Enable();
		_input.Drone.Disable();

		_input.General.Ekey.performed += InteractableArea_performed;
		_input.General.Ekey.started += InteractableArea_started;
		_input.General.Ekey.canceled += InteractableArea_canceled;

		_input.PlayerInput.SwitchToDrone.performed += SwitchToDrone_performed;

		_input.Drone.SwitchToPlayer.performed += SwitchToPlayer_performed;
		_input.Drone.SpaceKey.performed += SpaceKey_performed;
		_input.Drone.VKey.performed += VKey_performed;
		
	}

	#region //Input Events
	private void VKey_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		OnVKeyDronePressed?.Invoke();
	}

	private void SpaceKey_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		OnSpaceKeyDronePressed?.Invoke();
	}

	private void SwitchToPlayer_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		_input.PlayerInput.Enable();
		_input.Drone.Disable();
	}

	private void SwitchToDrone_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		_input.PlayerInput.Disable();
		_input.Drone.Enable();
	}

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
		DroneRotation();
		DroneTilt();
    }

	private void PlayerMove()
	{
		Vector2 move;
		move = _input.PlayerInput.Move.ReadValue<Vector2>();

		OnPlayerMove?.Invoke(move);
	}

	private void DroneRotation()
	{
		float rot;
		
		rot = _input.Drone.Rotation.ReadValue<float>();

		OnDroneRotation?.Invoke(rot);
	}

	private void DroneTilt()
	{
		Vector2 tilt;
		tilt = _input.Drone.Tilt.ReadValue<Vector2>();

		OnTiltDrone?.Invoke(tilt);
	}
}
