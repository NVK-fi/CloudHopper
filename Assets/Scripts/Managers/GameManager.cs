using UnityEngine;

namespace Managers
{
	using Platforms;
	using Player;
	using Settings;

	[DefaultExecutionOrder(-1)]
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance;
		public enum GameState { Paused, Playing, End }
		public GameState State { get; private set; }
		public int Score { get; private set; }
		
		[field: SerializeField] public ProgressionSettings ProgressionSettings { get; private set; }

		private void Awake()
		{
			// Singleton pattern.
			if (Instance == null)
				Instance = this;
			else
				Destroy(gameObject);
			
			if (ProgressionSettings == null)
				Debug.LogError("No ProgressionProgressionSettings set on " + this + "!");
		}

		private void OnEnable() => Player.Instance.PlatformTouched += OnPlatformTouched;

		private void OnDisable() => Player.Instance.PlatformTouched -= OnPlatformTouched;

		private void OnPlatformTouched(Platform _) => Score++;
	}
}
