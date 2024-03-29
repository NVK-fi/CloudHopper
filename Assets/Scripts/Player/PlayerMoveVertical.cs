using UnityEngine;

namespace Player
{
	using Managers;
	using Platforms;
	using Settings;
	using Tools;
	using UnityEngine.InputSystem;

	[RequireComponent(typeof(Player))]
	public class PlayerMoveVertical : MonoBehaviour
	{
		private Player _player;
		private ProgressionSettings _progression;
		private PhysicsSettings _physics;
		private bool _canDive;

		private void Awake()
		{
			_player = GetComponent<Player>();
			_progression = GameManager.Instance.ProgressionSettings;
			_physics = _player.Physics;
		}

		private void Update() => ApplyGravity();

		private void OnEnable()
		{
			_player.Controls.InGame.Dive.started += TryDive;
			_player.PlatformTouched += OnPlatformTouched;
		}

		private void OnDisable()
		{
			_player.Controls.InGame.Dive.started -= TryDive;
			_player.PlatformTouched -= OnPlatformTouched;
		}

		private void OnPlatformTouched(Platform _) => Hop();

		private void ApplyGravity() => _player.LocalVelocity += Vector3.down * (_physics.Gravity * Time.deltaTime);

		private void Hop()
		{
			// Apply the progression factor to hopping only after the initial platforms.
			var hopsAfterWarmup = Mathf.Max(0, GameManager.Instance.Score - PlatformManager.PlatformsCount);
			
			var hopVelocity = _physics.HopVelocity * ProgressionMultipliers.VerticalVelocity(hopsAfterWarmup);
			_player.LocalVelocity = _player.LocalVelocity.With(y: hopVelocity);
		}

		private void TryDive(InputAction.CallbackContext _)
		{
			// Apply the progression factor before the next check, but only after the initial platforms.
			var hopsAfterWarmup = Mathf.Max(0, GameManager.Instance.Score - PlatformManager.PlatformsCount);
			var diveVelocity = _physics.DiveVelocity * ProgressionMultipliers.VerticalVelocity(hopsAfterWarmup);
			var hopVelocity = _physics.HopVelocity * ProgressionMultipliers.VerticalVelocity(hopsAfterWarmup);
			
			// Make sure the player has not just hopped or already dived.
			if (_player.LocalVelocity.y.IsBetween(-diveVelocity, hopVelocity * .9f)) 
				_player.LocalVelocity = _player.LocalVelocity.With(y: -diveVelocity);
		}
	}
}