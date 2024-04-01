using UnityEngine;

namespace Platforms
{
	using Managers;
	using Player;
	using Settings;
	using Random = Random;

	public class PlatformManager : MonoBehaviour
	{
		[SerializeField] private Platform furthestPlatform;

		private ProgressionSettings _progression;
		private PhysicsSettings _physics;
		private int _platformsCount;

		private void Awake()
		{
			if (furthestPlatform == null)
			{
				Debug.LogError("No furthest platform set on " + this + "!");
				enabled = false;
			}

			_progression = GameManager.Instance.ProgressionSettings;
			_physics = Player.Instance.Physics;
			_platformsCount = GetComponentsInChildren<Platform>().Length;
		}

		private void OnEnable() => Player.Instance.PlatformTouched += OnPlatformTouched;

		private void OnDisable() => Player.Instance.PlatformTouched -= OnPlatformTouched;

		private void OnPlatformTouched(Platform touchedPlatform)
		{
			touchedPlatform.transform.position = NextPlatformPosition();
			furthestPlatform = touchedPlatform;
		}
		
		/// <summary>
		/// Calculates the position for the next platform from the current furthest platform.
		/// The position is calculated using the forward and hopping velocities from the physics settings,
		/// making it always possible to reach by player (in theory).
		/// </summary>
		private Vector3 NextPlatformPosition()
		{
			var additionalDistance = Vector3.forward * MaxHopDistance() * .8f;
			var randomizedPosition = Random.insideUnitSphere * MaxHopDistance() * .15f;
			
			return furthestPlatform.transform.position + additionalDistance + randomizedPosition;
		}

		private float MaxHopDistance()
		{
			// Calculate the forward and hopping velocities with progression factor.
			var scoreWithOffset = GameManager.Instance.Score + _platformsCount - 1;
			var forwardVelocity = _physics.ForwardVelocity * _progression.ForwardVelocity(scoreWithOffset);
			var hopVelocity = _physics.HopVelocity * _progression.VerticalVelocity(scoreWithOffset);

			// The formula for maximum hop distance is "(2*f*h)/g", where f is forward velocity, h is hop velocity, and g is gravity.
			// - Or so I hope, I'm learning stuff as I go.
			return (2 * forwardVelocity * hopVelocity) / _physics.Gravity;
		}
	}
}