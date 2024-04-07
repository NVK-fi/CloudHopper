using UnityEngine;

namespace Player
{
	using System;
	using System.Collections;
	using Controls;
	using Game;
	using Platforms;
	using Settings;

	[RequireComponent(typeof(CharacterController))]
	[DefaultExecutionOrder(-2)]
	public class Player : MonoBehaviour
	{
		public event Action PlayerDeath;
		public event Action<Platform> PlatformTouched;
		public event Action PlayerTooFar;
		public static Player Instance { get; private set; }
		public InputAsset Controls { get; private set; }
		[field: SerializeField] public PhysicsSettings PhysicsSettings { get; private set; }
		[SerializeField] private ProgressionSettings progressionSettings;

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
			Controls = new InputAsset();
			
			if (PhysicsSettings == null)
				Debug.LogError("No PhysicsSettings set on " + this + "!");

			if (progressionSettings == null)
				Debug.LogError("No ProgressionSettings set on " + this + "!");
		}

		private void OnEnable() => Controls.Enable();

		private void OnDisable() => Controls.Disable();

		private void LateUpdate()
		{
			_controller.Move(transform.rotation * LocalVelocity * Time.deltaTime);
			
			// Invoke the death event.
			if (transform.position.y < _deathHeight && LocalVelocity.y < 0) 
				PlayerDeath?.Invoke();
		}

		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			if (_isColliderCooldownActive) return;
			
			var platform = hit.gameObject.GetComponentInParent<Platform>();
			if (platform == null) return;

			StartCoroutine(ColliderCooldown());
			PlatformTouched?.Invoke(platform);
			
			// Reset the position if the player has moved too far from the origin.
			if (transform.position.z > Constants.MAX_PLAYER_DISTANCE)
			{
				PlayerTooFar?.Invoke();
				_controller.Move(-transform.position);
			}
			
			// Update the DeathHeight from the lowest platform.
			_deathHeight = GameManager.Instance.Platforms.GetTheLowestPlatformHeight() - 10f;
		}

		/// <summary>
		/// Cooldown method used for preventing multiple collider hits within a small time frame.
		/// - I don't know if toggling an extern boolean Controller.detectCollisions would be bad, probably.
		/// </summary>
		private IEnumerator ColliderCooldown()
		{
			_isColliderCooldownActive = true;
			yield return new WaitForSeconds(0.1f);
			_isColliderCooldownActive = false;
		}

		public enum Direction { Forward, Vertical }

		/// <summary>
		/// Calculates a progression multiplier at given hop count.
		/// - Starts from 1 and goes up a small increment after each hop based on ProgressionSettings.
		/// </summary>
		public float GetProgressionMultiplier(Direction direction, int hopCount)
		{
			// Cap the progression to a huge number so things won't reach infinity.
			// - I don't expect anyone to get this far.
			hopCount = Mathf.Min(hopCount, Constants.MAX_PROGRESSION);
			
			var increment = direction == Direction.Forward
				? progressionSettings.ForwardIncrement 
				: progressionSettings.VerticalIncrement;
			
			return 1f + hopCount * increment;
		}
	}
}
