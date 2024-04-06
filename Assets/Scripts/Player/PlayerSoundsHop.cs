using UnityEngine;

namespace Player
{
	using Game;
	using Platforms;
	using Random = Random;

	public class PlayerSoundsHop : MonoBehaviour
	{
		[SerializeField] private float pitchVariety;
		[SerializeField] private AudioSource hopSoundSource;
		[SerializeField] private AudioSource boostSoundSource;

		private void OnEnable()
		{
			Player.Instance.PlatformTouched += OnPlatformTouched;
			GameManager.Instance.PlatformManager.PlatformSkipped += OnPlatformSkipped;
		}

		private void OnDisable()
		{
			Player.Instance.PlatformTouched -= OnPlatformTouched;
			GameManager.Instance.PlatformManager.PlatformSkipped -= OnPlatformSkipped;
		}

		private void OnPlatformTouched(Platform _) => PlaySoundWithRandomPitch(hopSoundSource);

		private void OnPlatformSkipped() => PlaySoundWithRandomPitch(boostSoundSource);

		private void PlaySoundWithRandomPitch(AudioSource audioSource)
		{
			audioSource.Stop();
			audioSource.pitch = Random.Range(1f - pitchVariety * .5f, 1f + pitchVariety * .5f);
			audioSource.Play();
		}
	}
}
