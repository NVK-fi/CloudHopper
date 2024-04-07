using UnityEngine;

namespace Player
{
	using Game;
	using Platforms;
	using Settings;
	using Tools;
	using UnityEngine.InputSystem;

	/// <summary>
	/// Moves the player vertically by handling hopping, diving and the gravity of the situation.
	/// </summary>
	public class PlayerMoveVertical : MonoBehaviour
	{
		private Player _player;
		private PhysicsSettings _physicsSettings;

		private void Awake()
		{
			_player = Player.Instance;
			_physicsSettings = _player.PhysicsSettings;
		}

		private void Update() => ApplyGravity();

		private void OnEnable()
		{
			_player.Controls.InGame.Dive.started += TryDive;
			_player.PlatformTouched += Hop;
		}

		private void OnDisable()
		{
			_player.Controls.InGame.Dive.started -= TryDive;
			_player.PlatformTouched -= Hop;
		}

		private void ApplyGravity() => _player.LocalVelocity += Vector3.down * (_physicsSettings.Gravity * Time.deltaTime);

		private void Hop(Platform _)
		{
			var progressionMultiplier = _player.GetProgressionMultiplier(Player.Direction.Vertical, GameManager.Instance.Score.Current);
			var hopVelocity = _physicsSettings.HopVelocity * progressionMultiplier;
			
			_player.LocalVelocity = _player.LocalVelocity.With(y: hopVelocity);
		}

		private void TryDive(InputAction.CallbackContext _)
		{
			var progressionMultiplier = _player.GetProgressionMultiplier(Player.Direction.Vertical, GameManager.Instance.Score.Current);
			var diveVelocity = _physicsSettings.DiveVelocity * progressionMultiplier;
			var hopVelocity = _physicsSettings.HopVelocity * progressionMultiplier;
			
			// Make sure the player is not already diving or had not just hopped.
			if (!_player.LocalVelocity.y.IsBetween(-diveVelocity, hopVelocity * .9f)) return;
			
			_player.LocalVelocity = _player.LocalVelocity.With(y: -diveVelocity);
		}
	}
}