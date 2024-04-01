using UnityEngine;

namespace Player
{
	using Platforms;

	[RequireComponent(typeof(AudioSource))]
	public class PlayerSoundsDive : MonoBehaviour
	{
		[SerializeField] private float lowVolume = .1f;
		[SerializeField] private float highVolume = .5f;
		[SerializeField] private float lowPitch = 0.9f;
		[SerializeField] private float highPitch = 1.1f;
		[SerializeField] private PlayerMoveVertical playerMoveVertical;

		private AudioSource _audioSource;

		private void Awake()
		{
			_audioSource = GetComponent<AudioSource>();
			_audioSource.volume = lowVolume;

			if (playerMoveVertical == null)
			{
				Debug.LogError("No PlayerMoveVertical attached to " + this + "!");
				enabled = false;
			}
		}

		private void OnEnable()
		{
			playerMoveVertical.DivingStarted += OnDivingStarted;
			Player.Instance.PlatformTouched += OnPlatformTouched;
		}

		private void OnDisable()
		{
			playerMoveVertical.DivingStarted -= OnDivingStarted;
			Player.Instance.PlatformTouched -= OnPlatformTouched;
		}

		private void OnPlatformTouched(Platform _)
		{
			_audioSource.volume = lowVolume;
			_audioSource.pitch = lowPitch;
		}

		private void OnDivingStarted()
		{
			_audioSource.volume = highVolume;
			_audioSource.pitch = highPitch;
		}
	}
}
