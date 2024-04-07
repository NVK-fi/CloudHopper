using UnityEngine;

namespace Platforms
{
	using System;
	using System.Linq;
	using Game;
	using Settings;
	using Tools;
	using Random = Random;

	/// <summary>
	/// Serves as a parent class for individual Platforms,
	/// and is responsible for handling their repositioning calculations.
	/// </summary>
	public class PlatformManager : MonoBehaviour
	{
		public event Action<int> PlatformsSkipped;
		
		private Platform[] _platforms;
		private Platform _furthestPlatform;

		private void Awake()
		{
			_platforms = GetComponentsInChildren<Platform>();

			if (_platforms == null || _platforms.Length < 1)
			{
				Debug.LogError("No Platform(s) found on " + this + "!");
				enabled = false;
				return;
			}

			// Calculate the furthest platform.
			var furthestDistance = Mathf.NegativeInfinity;
			foreach (var platform in _platforms)
			{
				if (platform.transform.position.z <= furthestDistance) continue;
				
				furthestDistance = platform.transform.position.z;
				_furthestPlatform = platform;
			}
		}

		private void OnEnable()
		{
			Game.Instance.Player.TouchedPlatform += OnPlayerTouchedPlatform;
			Game.Instance.Player.ReachedMaxDistance += OnPlayerReachedMaxDistance;
		}

		private void OnDisable()
		{
			Game.Instance.Player.TouchedPlatform -= OnPlayerTouchedPlatform;
			Game.Instance.Player.ReachedMaxDistance += OnPlayerReachedMaxDistance;
		}


		private void OnPlayerTouchedPlatform(Platform touchedPlatform)
		{
			if (touchedPlatform == null) return;
			
			// All platforms left behind are moved first.
			MoveSkippedPlatforms(touchedPlatform);
			
			// Move the current platform to a new position.
			touchedPlatform.TeleportTo(CalculateNextPlatformPosition());
			_furthestPlatform = touchedPlatform;
		}

		private void MoveSkippedPlatforms(Platform touchedPlatform)
		{
			var platformsSkipped = 0;
			foreach (var platform in _platforms)
			{
				if (platform.transform.position.z >= touchedPlatform.transform.position.z) continue;

				platform.TeleportTo(CalculateNextPlatformPosition());
				_furthestPlatform = platform;
				platformsSkipped++;
			}
			
			if (platformsSkipped > 0) PlatformsSkipped?.Invoke(platformsSkipped);
		}

		/// <summary>
		/// Calculates a new position from the furthest platform.
		/// This position is always reachable by player (in theory).
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
			
			return _furthestPlatform.transform.position + nextPosition;
		}

		/// <summary>
		/// Calculates the maximum hop distance based on forward and hop velocities.
		/// </summary>
		private float MaxHopDistance()
		{
			// Calculate the forward and hopping velocities with progression factors.
			var progressionSettings = Game.Instance.ProgressionSettings;
			var physicsSettings = Game.Instance.PhysicsSettings;
			var scoreWithOffset = Game.Instance.Score.Current + _platforms.Length - 1;

			var forwardMultiplier = progressionSettings.ForwardMultiplier(scoreWithOffset);
			var forwardVelocity = physicsSettings.ForwardVelocity * forwardMultiplier;

			var verticalMultiplier = progressionSettings.VerticalMultiplier(scoreWithOffset);
			var hopVelocity = physicsSettings.HopVelocity * verticalMultiplier;

			// The formula for maximum hop distance is "(2*f*h)/g", where f is forward velocity, h is hop velocity, and g is gravity.
			// - Or so I hope, I'm learning stuff as I go.
			return (2 * forwardVelocity * hopVelocity) / physicsSettings.Gravity;
		}

		/// <summary>
		/// Gets the height of the lowest platform.
		/// </summary>
		public float GetTheLowestPlatformHeight()
		{
			return _platforms != null && _platforms.Length > 0 
				? _platforms.Min(platform => platform.transform.position.y) 
				: 0f;
		}

		/// <summary>
		/// Moves the platforms back towards the origin preventing issues with floating point accuracy.
		/// - I doubt anyone will ever get this far without cheating.
		/// </summary>
		private void OnPlayerReachedMaxDistance()
		{
			foreach (var platform in _platforms) 
				platform.transform.position -= Game.Instance.Player.transform.position;
		}
	}
}