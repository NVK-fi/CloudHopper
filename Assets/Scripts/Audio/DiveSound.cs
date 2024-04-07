namespace Audio
{
	using Player;
	using UnityEngine;

	/// <summary>
	/// Modifies the PersistentWindLoop by changing its volume and pitch when the player falls.
	/// The changes are proportional to player's falling velocity.
	/// </summary>
	public class DiveSound : MonoBehaviour
	{
		[SerializeField] private float volumeMultiplier = 2f;
		[SerializeField] private float pitchMultiplier = 1.08f;
		[SerializeField] private float updateInterval = 0.05f;

		private AudioSource _audioSource;

		private bool _isAlive = true;
		private float _timer;
		
		private float _highPitch;
		private float _highVolume;
		private float _normalPitch;
		private float _normalVolume;

		private float _diveVelocity;
		private float _previousInterpolation;

		private void Awake()
		{
			_audioSource = PersistentWindLoop.Instance.AudioSource;

			if (_audioSource == null)
			{
				Debug.LogError("No persistent audio source found!");
				enabled = false;
			}
		}

		private void Start()
		{
			_normalVolume = _audioSource.volume;
			_normalPitch = _audioSource.pitch;
			_highVolume = _normalVolume * volumeMultiplier;
			_highPitch = _normalPitch * pitchMultiplier;

			_diveVelocity = Player.Instance.PhysicsSettings.DiveVelocity;
		}

		private void OnEnable()
		{
			Player.Instance.PlayerDeath += OnPlayerDeath;
		}

		private void OnDisable()
		{
			Player.Instance.PlayerDeath -= OnPlayerDeath;

			if (_audioSource == null) return;
			_audioSource.volume = _normalVolume;
			_audioSource.pitch = _normalPitch;
		}

		private void OnPlayerDeath() => _isAlive = false;

		private void Update()
		{
			// Use a custom update interval.
			_timer += Time.unscaledDeltaTime;
			if (_timer < updateInterval) return;
			_timer = 0f;

			var velocity = _isAlive ? Player.Instance.LocalVelocity.y : 0f;
			LerpPitchAndVolume(velocity);
		}

		private void LerpPitchAndVolume(float velocity)
		{
			// Create an interpolation value from the velocity.
			// - From zero to one, how much are we falling?
			var currentInterpolation = Mathf.InverseLerp(0f, -_diveVelocity, velocity);
			if (Mathf.Abs(currentInterpolation - _previousInterpolation) < 0.01f) return;
				
			// Create a value between previous and current interpolations, so changes are not immediate.
			var newInterpolation = Mathf.Lerp(_previousInterpolation, currentInterpolation, .5f);
			_previousInterpolation = newInterpolation;

			// Finally, apply the new interpolation for a gradual change.
			_audioSource.volume = Mathf.Lerp(_normalVolume, _highVolume, newInterpolation);
			_audioSource.pitch = Mathf.Lerp(_normalPitch, _highPitch, newInterpolation);
		}
	}
}