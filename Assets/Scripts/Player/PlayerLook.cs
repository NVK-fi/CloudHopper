using UnityEngine;

namespace Player
{
	using System.Collections;
	using Settings;
	using UnityEngine.InputSystem;

	[RequireComponent(typeof(Player))]
	public class PlayerLook : MonoBehaviour
	{
		[SerializeField] private Camera playerCamera;

		private Player _player;
		private PlayerControls _controls;
		private float _cameraVerticalAngle;
		private float _sensitivity;
		
		private bool _isInputBlocked;
		private Coroutine _inputBlockCoroutine;
		
		private void Awake()
		{
			_player = GetComponent<Player>();
			_controls = _player.Controls;
			
			_cameraVerticalAngle = playerCamera.transform.localEulerAngles.x;

			var sensitivityPower = PlayerPrefs.GetInt(SettingsStrings.Sensitivity, 5);
			_sensitivity = Mathf.Pow(1.5f, sensitivityPower);
		}

		private void OnEnable()
		{
			HideCursor(true);
			_controls.InGame.Look.performed += Look;

			_inputBlockCoroutine = StartCoroutine(BlockInput(.5f));
		}
		
		private void OnDisable()
		{
			HideCursor(false);
			_controls.InGame.Look.performed -= Look;
			
			if (_inputBlockCoroutine != null)
				StopCoroutine(_inputBlockCoroutine);
		}

		/// <summary>
		/// Rotates the Player horizontally and the Camera vertically based on user input.
		/// </summary>
		private void Look(InputAction.CallbackContext context)
		{
			if (_isInputBlocked) return;
			
			var input = context.ReadValue<Vector2>();

			// Rotates the player horizontally.
			transform.Rotate(Vector3.up * input.x * (_sensitivity * Time.unscaledDeltaTime), Space.Self);
			
			// Rotates the camera vertically while clamping the angle.
			_cameraVerticalAngle -= input.y * (_sensitivity * Time.unscaledDeltaTime);
			_cameraVerticalAngle = Mathf.Clamp(_cameraVerticalAngle, -89f, 89f);
			playerCamera.transform.localEulerAngles = new Vector3(_cameraVerticalAngle, 0, 0);
		}

		/// <summary>
		/// Hides or shows the cursor.
		/// </summary>
		private static void HideCursor(bool hide)
		{
			if (hide)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			else
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		private IEnumerator BlockInput(float seconds)
		{
			_isInputBlocked = true;
			yield return new WaitForSecondsRealtime(seconds);
			_isInputBlocked = false;
			
			_inputBlockCoroutine = null;
		}
	}
}
