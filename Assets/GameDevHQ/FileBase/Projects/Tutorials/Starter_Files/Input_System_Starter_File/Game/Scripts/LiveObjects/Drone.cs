using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Game.Scripts.UI;
using UnityEngine.InputSystem;

namespace Game.Scripts.LiveObjects
{
	public class Drone : MonoBehaviour
	{
		private enum Tilt
		{
			NoTilt, Forward, Back, Left, Right
		}

		[SerializeField] private Rigidbody _rigidbody;
		[SerializeField] private float _speed = 5f;
		private bool _inFlightMode = false;
		[SerializeField] private Animator _propAnim;
		[SerializeField] private CinemachineVirtualCamera _droneCam;
		[SerializeField] private InteractableZone _interactableZone;

		private Vector2 _tilt;

		public static event Action OnEnterFlightMode;
		public static event Action onExitFlightmode;

		private void OnEnable()
		{
			InteractableZone.onZoneInteractionComplete += EnterFlightMode;
			GameInputManager.OnDroneRotation += CalculateMovementUpdate;
			GameInputManager.OnTiltDrone += TiltValues;
			GameInputManager.OnSpaceKeyDronePressed += CalculateMoveFXUpdateSpaceKey;
			GameInputManager.OnVKeyDronePressed += CalculateMoveFXUpdateVKey;
		}

		private void Update()
		{
			if (_inFlightMode)
			{
				CalculateTilt();

				if (Keyboard.current.escapeKey.wasPressedThisFrame)
				{
					_inFlightMode = false;
					onExitFlightmode?.Invoke();
					ExitFlightMode();
				}
			}
		}

		private void FixedUpdate()
		{
			_rigidbody.AddForce(transform.up * (9.81f), ForceMode.Acceleration);
		}

		private void ExitFlightMode()
		{
			_droneCam.Priority = 9;
			_inFlightMode = false;
			UIManager.Instance.DroneView(false);
		}

		private void EnterFlightMode(InteractableZone zone)
		{
			if (_inFlightMode != true && zone.GetZoneID() == 4) // drone Scene
			{
				_propAnim.SetTrigger("StartProps");
				_droneCam.Priority = 11;
				_inFlightMode = true;
				OnEnterFlightMode?.Invoke();
				UIManager.Instance.DroneView(true);
				_interactableZone.CompleteTask(4);
			}
		}

		private void CalculateMovementUpdate(float rot)
		{
			if (rot < 0)
			{
				var tempRot = transform.localRotation.eulerAngles;
				tempRot.y -= _speed / 3;
				transform.localRotation = Quaternion.Euler(tempRot);
			}
			if (rot > 0)
			{
				var tempRot = transform.localRotation.eulerAngles;
				tempRot.y += _speed / 3;
				transform.localRotation = Quaternion.Euler(tempRot);
			}
		}

		private void CalculateMoveFXUpdateSpaceKey()
		{
			if (_inFlightMode)
			{
				_rigidbody.AddForce(transform.up * _speed, ForceMode.Acceleration);
			}
		}

		private void CalculateMoveFXUpdateVKey()
		{
			if (_inFlightMode)
			{
				_rigidbody.AddForce(-transform.up * _speed, ForceMode.Acceleration);
			}
		}

		private void TiltValues(Vector2 tilt)
		{
			_tilt = tilt;
		}

		private void CalculateTilt()
		{
			if (_tilt.x < 0)
				transform.rotation = Quaternion.Euler(00, transform.localRotation.eulerAngles.y, 30);
			else if (_tilt.x > 0)
				transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, -30);
			else if (_tilt.y > 0)
				transform.rotation = Quaternion.Euler(30, transform.localRotation.eulerAngles.y, 0);
			else if (_tilt.y < 0)
				transform.rotation = Quaternion.Euler(-30, transform.localRotation.eulerAngles.y, 0);
			else
				transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);
		}

		private void OnDisable()
		{
			InteractableZone.onZoneInteractionComplete -= EnterFlightMode;
			GameInputManager.OnDroneRotation -= CalculateMovementUpdate;
			GameInputManager.OnTiltDrone -= TiltValues;
			GameInputManager.OnSpaceKeyDronePressed -= CalculateMoveFXUpdateSpaceKey;
			GameInputManager.OnVKeyDronePressed -= CalculateMoveFXUpdateVKey;
		}
	}
}
