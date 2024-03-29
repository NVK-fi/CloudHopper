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
		[SerializeField] private float initialRandomness = 1f;

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
			MovePlatformFurther(touchedPlatform);
			furthestPlatform = touchedPlatform;
		}

		private void MovePlatformFurther(Platform platform)
		{
			platform.transform.position = CalculateNextPlatformPosition() + CalculateRandomOffset();
		}

		/// <summary>
		/// Calculates the position for the next platform from the current furthest platform.
		/// The position is calculated using the forward and hopping velocities from the physics settings,
		/// making it always possible to reach by player (in theory).
		/// </summary>
		private Vector3 CalculateNextPlatformPosition()
		{
			// Calculate the forward and hopping velocities with progression factor.
			var scoreWithOffset = GameManager.Instance.Score + _platformsCount - 1;
			var forwardVelocity = _physics.ForwardVelocity * _progression.ForwardVelocity(scoreWithOffset);
			var hopVelocity = _physics.HopVelocity * _progression.VerticalVelocity(scoreWithOffset);

			// The formula for maximum hop distance is "(2*f*h)/g", where f is forward velocity, h is hop velocity, and g is gravity.
			// - Or so I hope, I'm still learning as I go.
			var maxHopDistance = (2 * forwardVelocity * hopVelocity) / _physics.Gravity;
			
			print( "Score:" + GameManager.Instance.Score + " F:" + forwardVelocity + " H:" + hopVelocity + " = " + maxHopDistance);
			
			return furthestPlatform.transform.position + Vector3.forward * maxHopDistance;
		}

		/// <summary>
		/// Creates a random offset vector for the platform placement.
		/// </summary>
		private Vector3 CalculateRandomOffset()
		{
			var randomVector = Random.insideUnitSphere * initialRandomness * _progression.PlatformOffset(GameManager.Instance.Score);
			var randomY = Mathf.Min(randomVector.y, 1f) * .1f;
			var randomZ = Mathf.Min(randomVector.z, 0f) * .5f;
			return new Vector3(randomVector.x, randomY, randomZ);
		}

		
	}
}