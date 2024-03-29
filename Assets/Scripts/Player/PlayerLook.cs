using UnityEngine;

namespace Player
{
	using UnityEngine.InputSystem;

	[RequireComponent(typeof(Player))]
	public class PlayerLook : MonoBehaviour
	{
		[SerializeField] private float lookSensitivity = 10f;
		[SerializeField] private Camera playerCamera;

		private Player _player;
		private PlayerControls _controls;
		private float _cameraVerticalAngle;
		
		private void Awake()
		{
			_player = GetComponent<Player>();
			_controls = _player.Controls;
			
			_cameraVerticalAngle = playerCamera.transform.localEulerAngles.x;
		}

		private void OnEnable()
		{
			HideCursor(true);
			_controls.InGame.Look.performed += Look;
		}
		
		private void OnDisable()
		{
			HideCursor(false);
			_controls.InGame.Look.performed -= Look;
		}

		/// <summary>
		/// Rotates the Player horizontally and the Camera vertically based on user input.
		/// </summary>
		private void Look(InputAction.CallbackContext context)
		{
			var input = context.ReadValue<Vector2>();

			// Rotates the player horizontally.
			transform.Rotate(Vector3.up * input.x * (lookSensitivity * Time.unscaledDeltaTime), Space.Self);
			
			// Rotates the camera vertically while clamping the angle.
			_cameraVerticalAngle -= input.y * (lookSensitivity * Time.unscaledDeltaTime);
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
	}
}
