using UnityEngine;

namespace Tools
{
	[RequireComponent(typeof(AudioSource))]
	public class PersistentAudio : MonoBehaviour
	{
		public static PersistentAudio Instance { get; private set; }
		public AudioSource AudioSource { get; private set; }

		private void Awake()
		{
			// Singleton pattern.
			if (Instance == null)
				Instance = this;
			else
				Destroy(gameObject);
			
			// Make this instance persistent.
			DontDestroyOnLoad(gameObject);
			
			AudioSource = GetComponent<AudioSource>();
		}
	}
}
