namespace Audio
{
	using Game;
	using Platforms;
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
			Game.Instance.Player.TouchedPlatform += OnPlayerTouchedPlatform;
			Game.Instance.Platforms.PlatformsSkipped += OnPlatformsSkipped;
		}

		private void OnDisable()
		{
			Game.Instance.Player.TouchedPlatform -= OnPlayerTouchedPlatform;
			Game.Instance.Platforms.PlatformsSkipped -= OnPlatformsSkipped;
		}

		private void OnPlayerTouchedPlatform(Platform _) => PlaySoundWithRandomPitch(hopSoundSource);

		private void OnPlatformsSkipped(int _) => PlaySoundWithRandomPitch(boostSoundSource);

		private void PlaySoundWithRandomPitch(AudioSource audioSource)
		{
			audioSource.Stop();
			audioSource.pitch = Random.Range(1f - pitchVariety * .5f, 1f + pitchVariety * .5f);
			audioSource.Play();
		}
	}
}