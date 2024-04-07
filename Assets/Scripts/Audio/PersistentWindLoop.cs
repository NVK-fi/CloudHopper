namespace Audio
{
	using UnityEngine;

	/// <summary>
	/// Represents a persistent singleton with a looped wind audio source.
	/// The audio is meant to be played throughout the gameplay.
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class PersistentWindLoop : MonoBehaviour
	{
		[field: SerializeField] public AudioSource AudioSource { get; private set; }
		
		public static PersistentWindLoop Instance;

		private void Awake()
		{
			// Singleton pattern.
			if (Instance == null)
				Instance = this;
			else
				Destroy(gameObject);
			
			// Make this instance persistent.
			DontDestroyOnLoad(gameObject);
			
			if (AudioSource == null)
				Debug.LogError("No audio source attached on " + this + "!");
		}
	}
}
