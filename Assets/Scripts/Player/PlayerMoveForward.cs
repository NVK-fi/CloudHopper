using UnityEngine;

namespace Player
{
	using Managers;
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
			var forwardVelocity = _player.Physics.ForwardVelocity * CalculateForwardVelocityProgression();
			_player.LocalVelocity = _player.LocalVelocity.With(z: forwardVelocity);
		}
		
		/// <summary>
		/// A progression factor for player's forwards velocity.
		/// Starts from 1 and goes up a small amount after each hop after the first manually placed platforms.
		/// </summary>
		private float CalculateForwardVelocityProgression()
		{
			var hopsAfterWarmup = Mathf.Max(0, GameManager.Instance.Score - PlatformManager.PlatformsCount);
			return 1f + hopsAfterWarmup * _progression.PlayerForwardVelocity;
		}
		
		/// <summary>
		/// A progression factor for player's forwards velocity.
		/// Starts from 1 and goes up a small amount after each hop after the first manually placed platforms.
		/// </summary>
		private float PlayerForwardVelocityFactor => 1f + Mathf.Max(0, GameManager.Instance.Score - PlatformManager.PlatformsCount) * _progression.PlayerForwardVelocity;
	}
}
