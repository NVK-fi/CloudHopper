using UnityEngine;

namespace Player
{
	using System.Collections;
	using Controls;
	using Game;
	using Settings;
	using UnityEngine.InputSystem;

	/// <summary>
	/// Responsible for controlling the player's look direction in the game.
	/// </summary>
	[RequireComponent(typeof(Player))]
	public class PlayerLook : MonoBehaviour
	{
		[SerializeField] private Camera playerCamera;

		private InputAsset _controls;
		private float _cameraVerticalAngle;
		private float _sensitivity;
		
		private bool _isInputBlocked;
		private Coroutine _inputBlockCoroutine;
		
		private void Awake()
		{
			_controls = Game.Instance.Controls;
			_cameraVerticalAngle = playerCamera.transform.localEulerAngles.x;

			var sensitivityPower = PlayerPrefs.GetInt(Constants.SENSITIVITY_KEY, 5);
			_sensitivity = Mathf.Pow(1.5f, sensitivityPower);
		}

		private void OnEnable()
		{
			ShowCursor(false);
			_controls.InGame.Look.performed += OnLookPerformed;

			_inputBlockCoroutine = StartCoroutine(BlockInput(.5f));
		}
		
		private void OnDisable()
		{
			ShowCursor(true);
			_controls.InGame.Look.performed -= OnLookPerformed;
			
			if (_inputBlockCoroutine != null)
				StopCoroutine(_inputBlockCoroutine);
		}

		/// <summary>
		/// Rotates the Player horizontally and the Camera vertically based on user input.
		/// </summary>
		private void OnLookPerformed(InputAction.CallbackContext context)
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

		private static void ShowCursor(bool show)
		{
			Cursor.visible = show;
			Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
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
