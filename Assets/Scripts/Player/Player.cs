using UnityEngine;

namespace Player
{
	using System;
	using System.Collections;
	using Game;
	using Platforms;
	using Settings;

	/// <summary>
	/// Represents a player in the game.
	/// </summary>
	[RequireComponent(typeof(CharacterController))]
	public class Player : MonoBehaviour
	{
		public event Action Death;
		public event Action<Platform> TouchedPlatform;
		public event Action ReachedMaxDistance;

		/// <summary>
		/// Gets or sets the local velocity of the player.
		/// This velocity is forwarded to the Player's CharacterController.
		/// </summary>
		public Vector3 LocalVelocity { get; set; }

		private CharacterController _controller;
		private bool _isColliderCooldownActive;
		private float _deathHeight = -10f;

		private void Awake() => _controller = GetComponent<CharacterController>();

		private void LateUpdate()
		{
			// Moves the CharacterController using LocalVelocity.
			_controller.Move(transform.rotation * LocalVelocity * Time.deltaTime);
			
			// Invoke the death event.
			if (transform.position.y < _deathHeight && LocalVelocity.y < 0) 
				Death?.Invoke();
		}

		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			if (_isColliderCooldownActive) return;
			
			var platform = hit.gameObject.GetComponentInParent<Platform>();
			if (platform == null) return;

			TouchedPlatform?.Invoke(platform);
			StartCoroutine(ColliderCooldown());

			// Reset the position if the player has moved too far from the origin.
			// This is to prevent the loss of floating point accuracy.
			if (transform.position.z > Constants.MAX_PLAYER_DISTANCE)
			{
				ReachedMaxDistance?.Invoke();
				_controller.Move(-transform.position);
			}
			
			// Update the DeathHeight from the lowest platform.
			_deathHeight = Game.Instance.Platforms.GetTheLowestPlatformHeight() - 10f;
		}

		/// <summary>
		/// Cooldown method used for preventing multiple collider hits within a small time frame.
		/// - I don't know if toggling the extern boolean Controller.detectCollisions would be bad, probably.
		/// </summary>
		private IEnumerator ColliderCooldown()
		{
			_isColliderCooldownActive = true;
			yield return new WaitForSeconds(0.1f);
			_isColliderCooldownActive = false;
		}
	}
}
