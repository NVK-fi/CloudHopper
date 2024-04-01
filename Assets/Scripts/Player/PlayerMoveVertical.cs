using UnityEngine;

namespace Player
{
	using System;
	using Managers;
	using Platforms;
	using Settings;
	using Tools;
	using UnityEngine.InputSystem;

	[RequireComponent(typeof(Player))]
	public class PlayerMoveVertical : MonoBehaviour
	{
		public event Action DivingStarted;
		
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
			// Apply the progression factor to hopping.
			var hopVelocity = _physics.HopVelocity * _progression.VerticalVelocity(GameManager.Instance.Score);
			_player.LocalVelocity = _player.LocalVelocity.With(y: hopVelocity);
		}

		private void TryDive(InputAction.CallbackContext _)
		{
			// Apply the progression factors to velocities.
			var diveVelocity = _physics.DiveVelocity * _progression.VerticalVelocity(GameManager.Instance.Score);
			var hopVelocity = _physics.HopVelocity * _progression.VerticalVelocity(GameManager.Instance.Score);
			
			// Make sure the player has not just hopped or already is diving.
			if (!_player.LocalVelocity.y.IsBetween(-diveVelocity, hopVelocity * .9f)) return;
			
			_player.LocalVelocity = _player.LocalVelocity.With(y: -diveVelocity);
			DivingStarted?.Invoke();
		}
	}
}