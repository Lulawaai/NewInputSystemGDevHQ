using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace Game.Scripts.LiveObjects
{
	public class Crate : MonoBehaviour
	{
		[SerializeField] private float _punchDelay;
		[SerializeField] private GameObject _wholeCrate, _brokenCrate;
		[SerializeField] private Rigidbody[] _pieces;
		[SerializeField] private BoxCollider _crateCollider;
		[SerializeField] private InteractableZone _interactableZone;
		private bool _isReadyToBreak = false;
		[SerializeField] private bool _inzone = false;
		private bool _crateBroken = false;

		private float _force;
		[SerializeField]private float _radius;
		[SerializeField] private float _softForce;
		[SerializeField] private float _hardForce;

		private List<Rigidbody> _brakeOff = new List<Rigidbody>();

		public static event Action<bool> OnCrateZone;

		private void OnEnable()
		{
			_wholeCrate.SetActive(true);
			_brokenCrate.SetActive(false);
			InteractableZone.onZoneInteractionComplete += InteractableZone_onZoneInteractionComplete;
		}

		private void InteractableZone_onZoneInteractionComplete(InteractableZone zone)
		{
			_interactableZone = zone;
			if (_interactableZone.GetZoneID() == 6)
			{
				_inzone = true;
				OnCrateZone?.Invoke(_inzone);
			}
		}

		public void TabCrate(int tabOrHold)
		{
			if (_inzone == true)
			{
				if (tabOrHold == 0)
					_force = _softForce;

				else if (tabOrHold == 1)
					_force = _hardForce;

				if (_interactableZone.GetZoneID() == 6)
				{
					_wholeCrate.SetActive(false);
					_brokenCrate.SetActive(true);
				}

				ExplodeCrate();
			}
		}

		private void ExplodeCrate()
		{
			_crateBroken = true;

			Rigidbody[] rbCratePieces = _brokenCrate.GetComponentsInChildren<Rigidbody>();

			if (rbCratePieces.Length > 0)
			{
				foreach (var body in rbCratePieces)
				{
					Debug.Log("xplodeCrate");
					body.AddExplosionForce(_force, transform.position, _radius);
				}
			}

			_interactableZone.CompleteTask(6);
			Debug.Log("Completely Busted");
		}

		public bool CrateExploded()
		{
			return _crateBroken;
		}

		IEnumerator PunchDelay()
		{
			float delayTimer = 0;
			while (delayTimer < _punchDelay)
			{
				yield return new WaitForEndOfFrame();
				delayTimer += Time.deltaTime;
			}

			_interactableZone.ResetAction(6);
		}

		private void OnDisable()
		{
			InteractableZone.onZoneInteractionComplete -= InteractableZone_onZoneInteractionComplete;
		}
	}
}
