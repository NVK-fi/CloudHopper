namespace Audio
{
	using Game;
	using Platforms;
	using Player;
	using UnityEngine;
	using Random = UnityEngine.Random;

	/// <summary>
	/// This class is responsible for playing hop sound effects in the game.
	/// </summary>
	public class HopSounds : MonoBehaviour
	{
		[SerializeField] private float pitchVariety;
		[SerializeField] private AudioSource hopSoundSource;
		[SerializeField] private AudioSource boostSoundSource;

		private void OnEnable()
		{
			Player.Instance.PlatformTouched += OnPlatformTouched;
			GameManager.Instance.Platforms.PlatformSkipped += OnPlatformSkipped;
		}

		private void OnDisable()
		{
			Player.Instance.PlatformTouched -= OnPlatformTouched;
			GameManager.Instance.Platforms.PlatformSkipped -= OnPlatformSkipped;
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