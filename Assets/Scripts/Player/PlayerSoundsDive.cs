using UnityEngine;

namespace Player
{
	using System.Collections;
	using Tools;
	using UnityEngine.InputSystem;

	public class PlayerSoundsDive : MonoBehaviour
	{
		[SerializeField] private float updateInterval = 0.05f;
		[SerializeField] private float highVolume = .25f;
		[SerializeField] private float highPitch = 1.1f;

		private AudioSource _audioSource;
		private float _normalVolume;
		private float _normalPitch;

		private float _diveVelocity;
		private float _timer;
		private float _lastLerpValue;
		private bool _isPlayerDead;


		private void Start()
		{
			_audioSource = PersistentAudio.Instance.AudioSource;

			if (_audioSource == null)
			{
				Debug.LogError("No persistent audio source found!");
				enabled = false;
			}
			
			_normalVolume = _audioSource.volume;
			_normalPitch = _audioSource.pitch;
			_diveVelocity = Player.Instance.Physics.DiveVelocity;
		}

		private void OnEnable()
		{
			Player.Instance.PlayerDeath += OnPlayerDeath;
			Player.Instance.Controls.InGame.Dive.started += OnPlayerDive;
		}

		private void OnDisable()
		{
			Player.Instance.PlayerDeath -= OnPlayerDeath;
			Player.Instance.Controls.InGame.Dive.started -= OnPlayerDive;
		}

		private void OnPlayerDeath() => _isPlayerDead = true;

		private void OnPlayerDive(InputAction.CallbackContext _)
		{
			if (!_isPlayerDead)
				LerpPitchAndVolume();
		}

		private void Update()
		{
			// Custom update interval.
			_timer += Time.unscaledDeltaTime;
			if (_timer < updateInterval) return;
			_timer = 0f;

			if (!_isPlayerDead)
				LerpPitchAndVolume();
			else
				NormalizePitchAndVolume();
		}

		private void LerpPitchAndVolume()
		{
			var currentVelocity = Player.Instance.LocalVelocity.y;
			var lerpValue = Mathf.InverseLerp(0f, -_diveVelocity, currentVelocity);

			if (Mathf.Approximately(lerpValue, _lastLerpValue)) return;
			
			// Take the average of previous and current lerp values.
			lerpValue = (_lastLerpValue + lerpValue) * .5f;
			_lastLerpValue = lerpValue;
			
			_audioSource.volume = Mathf.Lerp(_normalVolume, highVolume, lerpValue);
			_audioSource.pitch = Mathf.Lerp(_normalPitch, highPitch, lerpValue);
		}

		private void NormalizePitchAndVolume()
		{
			_audioSource.volume = Mathf.Lerp(_normalVolume, _audioSource.volume, .7f);
			_audioSource.pitch = Mathf.Lerp(_normalPitch, _audioSource.pitch, .7f);
		}
	}
}
