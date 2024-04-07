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
			_game.Controls.InGame.Dive.started += OnDivePressed;
			_player.TouchedPlatform += OnPlayerTouchedPlatform;
		}

		private void OnDisable()
		{
			_game.Controls.InGame.Dive.started -= OnDivePressed;
			_player.TouchedPlatform -= OnPlayerTouchedPlatform;
		}

		private void OnPlayerTouchedPlatform(Platform _) => Hop();

		private void OnDivePressed(InputAction.CallbackContext _) => TryDive();
		
		private void ApplyGravity()
		{
			var gravity = Vector3.down * (_physicsSettings.Gravity * Time.deltaTime);

			_player.LocalVelocity += gravity;
		}

		private void Hop()
		{
			var progressionMultiplier = _game.ProgressionSettings.VerticalMultiplier(_game.Score.Current);
			var hopVelocity = _physicsSettings.HopVelocity * progressionMultiplier;
			
			_player.LocalVelocity = _player.LocalVelocity.With(y: hopVelocity);
		}

		private void TryDive()
		{
			var progressionMultiplier = _game.ProgressionSettings.VerticalMultiplier(_game.Score.Current);
			var diveVelocity = _physicsSettings.DiveVelocity * progressionMultiplier;
			var hopVelocity = _physicsSettings.HopVelocity * progressionMultiplier;
			
			// Make sure the player is not already diving or had not just hopped.
			if (!_player.LocalVelocity.y.IsBetween(-diveVelocity, hopVelocity * .9f)) return;
			
			_player.LocalVelocity = _player.LocalVelocity.With(y: -diveVelocity);
		}
	}
}