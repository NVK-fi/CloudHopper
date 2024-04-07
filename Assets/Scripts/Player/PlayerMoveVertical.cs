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
	[RequireComponent(typeof(Player))]
	public class PlayerMoveVertical : MonoBehaviour
	{
		private Game _game;
		private Player _player;
		private PhysicsSettings _physicsSettings;

		private void Awake()
		{
			_game = Game.Instance;
			_player = _game.Player;
			_physicsSettings = Game.Instance.PhysicsSettings;
		}

		private void Update() => ApplyGravity();

		private void OnEnable()
		{
			Game.Instance.Controls.InGame.Dive.started += TryDive;
			_player.TouchedPlatform += Hop;
		}

		private void OnDisable()
		{
			Game.Instance.Controls.InGame.Dive.started -= TryDive;
			_player.TouchedPlatform -= Hop;
		}

		private void ApplyGravity() => _player.LocalVelocity += Vector3.down * (_physicsSettings.Gravity * Time.deltaTime);

		private void Hop(Platform _)
		{
			var progressionMultiplier = _game.GetProgressionMultiplier(Game.Direction.Vertical, _game.Score.Current);
			var hopVelocity = _physicsSettings.HopVelocity * progressionMultiplier;
			
			_player.LocalVelocity = _player.LocalVelocity.With(y: hopVelocity);
		}

		private void TryDive(InputAction.CallbackContext _)
		{
			var progressionMultiplier = _game.GetProgressionMultiplier(Game.Direction.Vertical, _game.Score.Current);
			var diveVelocity = _physicsSettings.DiveVelocity * progressionMultiplier;
			var hopVelocity = _physicsSettings.HopVelocity * progressionMultiplier;
			
			// Make sure the player is not already diving or had not just hopped.
			if (!_player.LocalVelocity.y.IsBetween(-diveVelocity, hopVelocity * .9f)) return;
			
			_player.LocalVelocity = _player.LocalVelocity.With(y: -diveVelocity);
		}
	}
}