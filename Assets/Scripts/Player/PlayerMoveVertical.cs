using UnityEngine;

namespace Player
{
	using Game;
	using Platforms;
	using Settings;
	using Tools;
	using UnityEngine.InputSystem;

	[RequireComponent(typeof(Player))]
	public class PlayerMoveVertical : MonoBehaviour
	{
		private Player _player;
		private ProgressionSettings _progression;
		private PhysicsSettings _physics;

		private void Awake()
		{
			_player = GetComponent<Player>();
			_progression = GameManager.Instance.ProgressionSettings;
			_physics = _player.Physics;
		}

		private void Update() => ApplyGravity();

		private void OnEnable()
		{
			_player.Controls.InGame.Dive.started += TryDive;
			_player.PlatformTouched += OnPlatformTouched;
		}

		private void OnDisable()
		{
			_player.Controls.InGame.Dive.started -= TryDive;
			_player.PlatformTouched -= OnPlatformTouched;
		}

		private void OnPlatformTouched(Platform _) => Hop();

		/// <summary>
		/// Applies gravity to the player's local velocity.
		/// </summary>
		private void ApplyGravity() => _player.LocalVelocity += Vector3.down * (_physics.Gravity * Time.deltaTime);

		/// <summary>
		/// Performs a hop action.
		/// </summary>
		private void Hop()
		{
			// Apply the progression factor to hopping.
			var hopVelocity = _physics.HopVelocity * _progression.VerticalVelocity(GameManager.Instance.Score);
			_player.LocalVelocity = _player.LocalVelocity.With(y: hopVelocity);
		}

		/// <summary>
		/// Tries to make the player 'dive' by adjusting their vertical velocity if possible.
		/// </summary>
		private void TryDive(InputAction.CallbackContext _)
		{
			// Apply the progression factors to velocities.
			var diveVelocity = _physics.DiveVelocity * _progression.VerticalVelocity(GameManager.Instance.Score);
			var hopVelocity = _physics.HopVelocity * _progression.VerticalVelocity(GameManager.Instance.Score);
			
			// Make sure the player has not just hopped or already is diving.
			if (!_player.LocalVelocity.y.IsBetween(-diveVelocity, hopVelocity * .9f)) return;
			
			_player.LocalVelocity = _player.LocalVelocity.With(y: -diveVelocity);
		}
	}
}