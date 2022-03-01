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

		[Header("UIElements")]
		[SerializeField] private Text _northKey_txt;
		[SerializeField] private GameObject _xStick_GO;
		[SerializeField] private Text _eastKey_txt;

		private void Awake()
		{
			_instance = this;
		}

		private void Start()
		{
			_xStick_GO.SetActive(false);
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
					_northKey_txt.text = "E";
					_eastKey_txt.text = "Escape";
					_xStick_GO.SetActive(false);
					break;

				case 1:
					_playerInpSelection.SetActive(false);
					_droneInpSelection.SetActive(true);
					_forkliftInpSelection.SetActive(false);
					_northKey_txt.text = "V";
					_xStick_GO.SetActive(true);
					break;

				case 2:
					_playerInpSelection.SetActive(false);
					_droneInpSelection.SetActive(false);
					_forkliftInpSelection.SetActive(true);
					_northKey_txt.text = "U";
					_eastKey_txt.text = "Down";
					_xStick_GO.SetActive(false);
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
