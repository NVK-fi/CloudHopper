using UnityEngine;

namespace Player
{
	using Platforms;
	using Random = Random;

	[RequireComponent(typeof(AudioSource))]
	public class PlayerSoundsHop : MonoBehaviour
	{
		[SerializeField] private float pitchVariety;
		[SerializeField] private AudioClip[] clips;

		private AudioSource _source;

		private void Awake() => _source = GetComponent<AudioSource>();
		private void OnEnable() => Player.Instance.PlatformTouched += OnPlatformTouched;
		private void OnDisable() => Player.Instance.PlatformTouched -= OnPlatformTouched;

		private void OnPlatformTouched(Platform _)
		{
			PlayRandom();
		}

		private void PlayRandom()
		{
			if (clips.Length < 1) return;
			_source.Stop();
			
			var index = Random.Range(0, clips.Length);
			_source.clip = clips[index];

			var pitch = Random.Range(1f - pitchVariety * .5f, 1f + pitchVariety * .5f);
			_source.pitch = pitch;

			_source.Play();
		}
	}
}
