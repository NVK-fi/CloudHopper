using UnityEngine;

namespace Player
{
	using Game;
	using Platforms;
	using Settings;
	using Tools;

	[RequireComponent(typeof(Player))]
	public class PlayerMoveForward : MonoBehaviour
	{
		private Player _player;
		private ProgressionSettings _progression;
		
		private void Awake()
		{
			_player = GetComponent<Player>();
			_progression = GameManager.Instance.ProgressionSettings;
		}

		private void OnEnable() => _player.PlatformTouched += OnPlatformTouched;

		private void OnDisable() => _player.PlatformTouched -= OnPlatformTouched;

		private void OnPlatformTouched(Platform _) => UpdateForwardVelocity();

		private void UpdateForwardVelocity()
		{
			// Apply the progression factor to forward velocity. The player goes faster after each hop.
			var forwardVelocity = _player.Physics.ForwardVelocity * _progression.ForwardVelocity(GameManager.Instance.Score);
			
			_player.LocalVelocity = _player.LocalVelocity.With(z: forwardVelocity);
		}
	}
}
