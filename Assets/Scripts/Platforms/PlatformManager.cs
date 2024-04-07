using UnityEngine;

namespace Platforms
{
	using System;
	using System.Linq;
	using Game;
	using Player;
	using Settings;
	using Tools;
	using Random = Random;

	public class PlatformManager : MonoBehaviour
	{
		public event Action PlatformSkipped;
		
		[SerializeField] private Platform furthestPlatform;

		private Player _player;
		private PhysicsSettings _physicsSettings;
		private Platform[] _platforms;

		private void Awake()
		{
			if (furthestPlatform == null)
			{
				Debug.LogError("No furthest platform set on " + this + "!");
				enabled = false;
			}

			_player = Player.Instance;
			_physicsSettings = _player.PhysicsSettings;
			_platforms = GetComponentsInChildren<Platform>();
		}

		private void OnEnable()
		{
			_player.PlatformTouched += OnPlatformTouched;
			_player.PlayerTooFar += OnPlayerTooFar;
		}

		private void OnDisable()
		{
			_player.PlatformTouched -= OnPlatformTouched;
			_player.PlayerTooFar += OnPlayerTooFar;
		}


		private void OnPlatformTouched(Platform touchedPlatform)
		{
			// All platforms left behind are moved first.
			// This only happens when the player skips a platform.
			CheckForSkippedPlatforms(touchedPlatform);
			
			// Move the current platform to a new position.
			MovePlatformToNextPosition(touchedPlatform);
		}

		private void CheckForSkippedPlatforms(Platform touchedPlatform)
		{
			foreach (var platform in _platforms)
			{
				if (platform.transform.position.z < touchedPlatform.transform.position.z)
				{
					PlatformSkipped?.Invoke();
					MovePlatformToNextPosition(platform);
				}
			}
		}

		private void MovePlatformToNextPosition(Platform platform)
		{
			platform.TeleportTo(CalculateNextPlatformPosition());
			
			furthestPlatform = platform;
		}

		/// <summary>
		/// Calculates the position for the next platform from the current furthest platform.
		/// The position is calculated using the forward and hopping velocities from the physics settings, 
		/// making it always possible to reach by player (in theory).
		/// </summary>
		private Vector3 CalculateNextPlatformPosition()
		{
			var maxHopDistance = MaxHopDistance();
			var additionalDistance = Vector3.forward * maxHopDistance * .6f;
			var randomizedPosition = Random.insideUnitSphere * maxHopDistance * .3f;
			randomizedPosition = randomizedPosition.With(y: randomizedPosition.y * .9f);
			var nextPosition = additionalDistance + randomizedPosition;
			
			// Clamp the Vector3 to prevent issues with floating point precision.
			nextPosition = nextPosition.ClampUniform(-Constants.MAX_PLATFORM_DISTANCE, Constants.MAX_PLATFORM_DISTANCE);
			
			return furthestPlatform.transform.position + nextPosition;
		}

		private float MaxHopDistance()
		{
			// Calculate the forward and hopping velocities with progression factors.
			var scoreWithOffset = GameManager.Instance.Score.Current + _platforms.Length - 1;

			var forwardMultiplier = _player.GetProgressionMultiplier(Player.Direction.Forward, scoreWithOffset);
			var forwardVelocity = _physicsSettings.ForwardVelocity * forwardMultiplier;

			var verticalMultiplier = _player.GetProgressionMultiplier(Player.Direction.Vertical, scoreWithOffset);
			var hopVelocity = _physicsSettings.HopVelocity * verticalMultiplier;

			// The formula for maximum hop distance is "(2*f*h)/g", where f is forward velocity, h is hop velocity, and g is gravity.
			// - Or so I hope, I'm learning stuff as I go.
			return (2 * forwardVelocity * hopVelocity) / _physicsSettings.Gravity;
		}

		/// <summary>
		/// Gets the height of the lowest platform.
		/// </summary>
		/// <returns>Returns the Y-value of the lowest platform world position.</returns>
		public float GetTheLowestPlatformHeight()
		{
			if (_platforms.Length < 1) return 0f;
			
			var lowestHeight = _platforms.Min(platform => platform.transform.position.y);
			
			return lowestHeight;
		}

		/// <summary>
		/// Moves the platforms back towards the origin.
		/// </summary>
		private void OnPlayerTooFar()
		{
			foreach (var platform in _platforms) 
				platform.transform.position -= _player.transform.position;
		}
	}
}