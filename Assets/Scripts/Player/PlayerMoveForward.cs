using UnityEngine;

namespace Player
{
	using Game;
	using Platforms;
	using Tools;

	/// <summary>
	/// Updates the player's forward velocity based on game progression.
	/// </summary>
	[RequireComponent(typeof(Player))]
	public class PlayerMoveForward : MonoBehaviour
	{
		private Player _player;
		
		private void Awake() => _player = Game.Instance.Player;

		private void OnEnable() => _player.TouchedPlatform += OnPlatformTouched;

		private void OnDisable() => _player.TouchedPlatform -= OnPlatformTouched;

		private void OnPlatformTouched(Platform _) => UpdateForwardVelocity();

		private void UpdateForwardVelocity()
		{
			// Apply the progression factor to forward velocity. The player goes faster after each hop.
			var progressionMultiplier = Game.Instance.GetProgressionMultiplier(Game.Direction.Forward, Game.Instance.Score.Current);
			var forwardVelocity = Game.Instance.PhysicsSettings.ForwardVelocity * progressionMultiplier;
			
			_player.LocalVelocity = _player.LocalVelocity.With(z: forwardVelocity);
		}
	}
}