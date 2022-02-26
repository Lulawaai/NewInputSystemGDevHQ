using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Game.Scripts.UI;
using Game.Scripts.LiveObjects;

public class GameInputManager : MonoBehaviour
{
	private GameInputActions _input;

	public static event Action<Vector2> OnPlayerMove;
	public static event Action OnInteractableKeyE;
	public static event Action OnHoldKey;
	public static event Action OnHoldFinished;
	//Drone
	public static event Action<float> OnDroneRotation;
	public static event Action OnSpaceKeyDronePressed;
	public static event Action OnVKeyDronePressed;
	public static event Action<Vector2> OnTiltDrone;
	public static event Action<Vector2> OnForkliftMove;
	public static event Action OnDroneModeFinshed;

	//Forklift
	public static event Action OnForkliftUp;
	public static event Action OnForkliftDown;

	//Crate
	[SerializeField] private bool _crateExploded = false;
	private int _tabOrHold;
	[SerializeField] private Crate _crate;

	private void OnEnable()
	{
		
	}

	void Start()
	{
		_input = new GameInputActions();
		_input.Enable();
		_input.Drone.Disable();
		_input.Forklift.Disable();
		_input.Crate.Disable();

		_input.General.Ekey.performed += InteractableArea_performed;
		_input.General.Ekey.started += InteractableArea_started;
		_input.General.Ekey.canceled += InteractableArea_canceled;

		_input.PlayerInput.SwitchToDrone.performed += SwitchToDrone_performed;

		_input.Drone.SwitchToForklift.performed += SwitchToForklift_performed;
		_input.Drone.SpaceKey.performed += SpaceKey_performed;
		_input.Drone.VKey.performed += VKey_performed;
		_input.Drone.EscKey.performed += EscKey_performed;

		_input.Forklift.SwitchToPlayer.performed += SwitchToPlayer_performed;
		_input.Forklift.LiftUp.performed += LiftUp_performed;
		_input.Forklift.LiftDown.performed += LiftDown_performed;

		_input.Crate.Explode.performed += Explode_performed;
		_input.Crate.Explode.canceled += Explode_canceled;
	}

	private void Explode_canceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		_crateExploded = _crate.CrateExploded();

		if (_crateExploded == false && context.duration >= 1.0f)
		{
			Debug.Log("Explded cancelled:: hold:::");
			_tabOrHold = 1;
		
			_crate.TabCrate(_tabOrHold);
		}
	}

	private void Explode_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		_crateExploded = _crate.CrateExploded();
		if (_crateExploded == false)
		{
			Debug.Log("Explded Peformed:: tab:::");
			_tabOrHold = 0;
			_crate.TabCrate(_tabOrHold);
		}
	}

	#region //Forklift
	private void LiftDown_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		OnForkliftDown?.Invoke();
	}

	private void LiftUp_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		OnForkliftUp?.Invoke();
	}

	private void SwitchToPlayer_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		_input.PlayerInput.Enable();
		_input.Drone.Disable();
		_input.Forklift.Disable();

		UIManager.Instance.SelectedInput(0);
	}
	#endregion

	#region //Drone
	private void VKey_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		OnVKeyDronePressed?.Invoke();
	}

	private void SpaceKey_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		OnSpaceKeyDronePressed?.Invoke();
	}

	private void SwitchToForklift_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		_input.PlayerInput.Disable();
		_input.Drone.Disable();
		_input.Forklift.Enable();

		UIManager.Instance.SelectedInput(2);
	}

	private void EscKey_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		OnDroneModeFinshed?.Invoke();
	}
	#endregion

	#region //Player
	private void SwitchToDrone_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		_input.PlayerInput.Disable();
		_input.Drone.Enable();
		_input.Forklift.Disable();

		UIManager.Instance.SelectedInput(1);
	}
	#endregion

	#region //Interactable Area
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
		ForkliftMove();
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

	private void ForkliftMove()
	{
		Vector2 move;
		move = _input.Forklift.Move.ReadValue<Vector2>();

		OnForkliftMove?.Invoke(move);
	}
}
