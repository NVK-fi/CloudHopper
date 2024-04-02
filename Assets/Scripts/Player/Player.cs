using UnityEngine;

namespace Player
{
	using System;
	using System.Collections;
	using Managers;
	using Platforms;
	using Settings;

	[RequireComponent(typeof(CharacterController))]
	[DefaultExecutionOrder(-2)]
	public class Player : MonoBehaviour
	{
		public event Action PlayerDeath;
		public event Action<Platform> PlatformTouched;
		public static Player Instance { get; private set; }
		public PlayerControls Controls { get; private set; }
		[field: SerializeField] public PhysicsSettings Physics { get; private set; }
		public Vector3 LocalVelocity { get; set; }
		
		private CharacterController _controller;
		private bool _isColliderCooldownActive;
		private float _deathHeight = -10f;

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

		private void LateUpdate()
		{
			if (GameManager.Instance.State == GameManager.GameState.Dead) return;
			
			_controller.Move(transform.rotation * LocalVelocity * Time.deltaTime);
			
			// Invoke Player death event.
			if (transform.position.y < _deathHeight) 
				PlayerDeath?.Invoke();
		}

		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			if (_isColliderCooldownActive) return;
			
			var platform = hit.gameObject.GetComponentInParent<Platform>();
			if (platform == null) return;

			StartCoroutine(ColliderCooldown());
			PlatformTouched?.Invoke(platform);
			
			// Update the DeathHeight from the lowest platform.
			_deathHeight = GameManager.Instance.PlatformManager.GetLowestPlatformHeight() - 10f;
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
