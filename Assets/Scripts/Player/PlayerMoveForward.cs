using UnityEngine;

namespace Player
{
	using Game;
	using Platforms;
	using Tools;

	[RequireComponent(typeof(Player))]
	public class PlayerMoveForward : MonoBehaviour
	{
		private Player _player;
		
		private void Awake() => _player = Player.Instance;

		private void OnEnable() => _player.PlatformTouched += OnPlatformTouched;

		private void OnDisable() => _player.PlatformTouched -= OnPlatformTouched;

		private void OnPlatformTouched(Platform _) => UpdateForwardVelocity();

		private void UpdateForwardVelocity()
		{
			// Apply the progression factor to forward velocity. The player goes faster after each hop.
			var progressionMultiplier = _player.GetProgressionMultiplier(Player.Direction.Forward, GameManager.Instance.Score);
			var forwardVelocity = _player.PhysicsSettings.ForwardVelocity * progressionMultiplier;
			
			_player.LocalVelocity = _player.LocalVelocity.With(z: forwardVelocity);
		}
	}
}
