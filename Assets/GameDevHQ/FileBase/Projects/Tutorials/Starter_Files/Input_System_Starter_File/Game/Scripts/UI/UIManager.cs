using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class UIManager : MonoBehaviour
	{
		#region Singleton
		private static UIManager _instance;
		public static UIManager Instance
		{
			get
			{
				if (_instance == null)
					Debug.LogError("UI Manager is NULL.");

				return _instance;
			}
		}
		#endregion

		[SerializeField] private Text _interactableZone;
		[SerializeField] private Image _inventoryDisplay;
		[SerializeField] private RawImage _droneCamView;

		[Header("InputSelection")]
		[SerializeField] private GameObject _playerInpSelection;
		[SerializeField] private GameObject _droneInpSelection;
		[SerializeField] private GameObject _forkliftInpSelection;
		[SerializeField] private int _inputSelected;

		private void Awake()
		{
			_instance = this;
		}

		public void SelectedInput(int selection)
		{
			_inputSelected = selection;

			switch (_inputSelected)
			{
				case 0:
					_playerInpSelection.SetActive(true);
					_droneInpSelection.SetActive(false);
					_forkliftInpSelection.SetActive(false);
					break;

				case 1:
					_playerInpSelection.SetActive(false);
					_droneInpSelection.SetActive(true);
					_forkliftInpSelection.SetActive(false);
					break;

				case 2:
					_playerInpSelection.SetActive(false);
					_droneInpSelection.SetActive(false);
					_forkliftInpSelection.SetActive(true);
					break;
			}
		}

		public void DisplayInteractableZoneMessage(bool showMessage, string message = null)
		{
			_interactableZone.text = message;
			_interactableZone.gameObject.SetActive(showMessage);
		}

		public void UpdateInventoryDisplay(Sprite icon)
		{
			_inventoryDisplay.sprite = icon;
		}

		public void DroneView(bool Active)
		{
			_droneCamView.enabled = Active;
		}
	}
}
