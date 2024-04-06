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

		private ProgressionSettings _progression;
		private PhysicsSettings _physics;
		private Platform[] _platforms;

		private void Awake()
		{
			if (furthestPlatform == null)
			{
				Debug.LogError("No furthest platform set on " + this + "!");
				enabled = false;
			}

			_progression = GameManager.Instance.ProgressionSettings;
			_physics = Player.Instance.Physics;
			_platforms = GetComponentsInChildren<Platform>();
		}

		private void OnEnable() => Player.Instance.PlatformTouched += OnPlatformTouched;

		private void OnDisable() => Player.Instance.PlatformTouched -= OnPlatformTouched;

		
		private void OnPlatformTouched(Platform touchedPlatform)
		{
			// All platforms left behind are moved first.
			// This only happens when the player skips a platform.
			CheckForSkippedPlatforms(touchedPlatform);
			
			// Move the current platform to a new position.
			MovePlatformToNextPosition(touchedPlatform);
		}

		/// <summary>
		/// Checks for skipped platforms and moves them ahead if necessary.
		/// </summary>
		/// <param name="touchedPlatform">The platform that was touched by the player.</param>
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

		/// <summary>
		/// Moves the specified platform to be the furthest.
		/// </summary>
		/// <param name="platform">The platform to be moved.</param>
		private void MovePlatformToNextPosition(Platform platform)
		{
			var nextPosition = CalculateNextPlatformPosition();
			platform.TeleportTo(nextPosition);
			
			furthestPlatform = platform;
		}

		/// <summary>
		/// Calculates the position for the next platform from the current furthest platform.
		/// The position is calculated using the forward and hopping velocities from the physics settings, 
		/// making it always possible to reach by player (in theory).
		/// </summary>
		private Vector3 CalculateNextPlatformPosition()
		{
			var additionalDistance = Vector3.forward * MaxHopDistance() * .6f;
			var randomizedPosition = Random.insideUnitSphere * MaxHopDistance() * .3f;
			var nextPosition = furthestPlatform.transform.position + additionalDistance + randomizedPosition;
			
			// Clamp the Vector3 to prevent issues with floating point precision.
			nextPosition = nextPosition.ClampUniform(-10000f, 10000f);
			
			return nextPosition;
		}

		private float MaxHopDistance()
		{
			// Calculate the forward and hopping velocities with progression factor.
			var scoreWithOffset = GameManager.Instance.Score + _platforms.Length - 1;
			var forwardVelocity = _physics.ForwardVelocity * _progression.ForwardVelocity(scoreWithOffset);
			var hopVelocity = _physics.HopVelocity * _progression.VerticalVelocity(scoreWithOffset);

			// The formula for maximum hop distance is "(2*f*h)/g", where f is forward velocity, h is hop velocity, and g is gravity.
			// - Or so I hope, I'm learning stuff as I go.
			return (2 * forwardVelocity * hopVelocity) / _physics.Gravity;
		}

		/// <summary>
		/// Gets the height of the lowest platform.
		/// </summary>
		/// <returns>Returns the Y-value of the lowest platform world position.</returns>
		public float GetLowestPlatformHeight()
		{
			if (_platforms.Length < 1) return 0f;
			return _platforms.Min(platform => platform.transform.position.y);
		}
	}
}