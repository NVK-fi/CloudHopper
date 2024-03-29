using UnityEngine;

namespace Player
{
	using System;
	using System.Collections;
	using Platforms;
	using Settings;

	[RequireComponent(typeof(CharacterController))]
	[DefaultExecutionOrder(-2)]
	public class Player : MonoBehaviour
	{
		public event Action<Platform> PlatformTouched;
		public static Player Instance { get; private set; }
		public PlayerControls Controls { get; private set; }
		[field: SerializeField] public PhysicsSettings Physics { get; private set; }
		public Vector3 LocalVelocity { get; set; }
		
		private CharacterController _controller;
		private bool _isColliderCooldownActive;

		private void Awake()
		{
			// Singleton pattern.
			if (Instance == null) 
				Instance = this;
			else 
				Destroy(gameObject);
			
			_controller = GetComponent<CharacterController>();
			Controls = new PlayerControls();
			
			if (Physics == null)
				Debug.LogError("No Physics set on " + this + "!");
		}

		private void OnEnable() => Controls.Enable();

		private void OnDisable() => Controls.Disable();

		private void LateUpdate() => _controller.Move(transform.rotation * LocalVelocity * Time.deltaTime);

		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			if (_isColliderCooldownActive) return;

			var platform = hit.gameObject.GetComponent<Platform>();
			if (platform != null)
			{
				StartCoroutine(ColliderCooldown());
				PlatformTouched?.Invoke(platform);
			}
		}

		/// <summary>
		/// Cooldown method used for preventing multiple collider hits within a small time frame.
		/// 
		/// - I don't know if toggling an extern boolean Controller.detectCollisions would be bad, probably.
		/// </summary>
		private IEnumerator ColliderCooldown()
		{
			_isColliderCooldownActive = true;
			yield return new WaitForSeconds(0.1f);
			_isColliderCooldownActive = false;
		}
	}
}
