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
		public enum GameState { Playing, Dead }
		public GameState State { get; private set; } = GameState.Playing;
		public int Score { get; private set; }
		[field: SerializeField] public ProgressionSettings ProgressionSettings { get; private set; }
		public PlatformManager PlatformManager { get; private set; }

		private void Awake()
		{
			// Singleton pattern.
			if (Instance == null)
				Instance = this;
			else
				Destroy(gameObject);
			
			PlatformManager = FindObjectOfType<PlatformManager>();
			if (PlatformManager == null) 
				Debug.LogError("No PlatformManager found in the scene!");

			if (ProgressionSettings == null)
				Debug.LogError("No ProgressionProgressionSettings set on " + this + "!");
		}

		private void OnEnable()
		{
			Player.Instance.PlatformTouched += OnPlatformTouched;
			Player.Instance.PlayerDeath += OnPlayerDeath;
		
			if (PlatformManager != null) 
				PlatformManager.PlatformSkipped += OnPlatformSkipped;
		}

		private void OnDisable()
		{
			Player.Instance.PlatformTouched -= OnPlatformTouched;
			Player.Instance.PlayerDeath -= OnPlayerDeath;
			
			if (PlatformManager != null) 
				PlatformManager.PlatformSkipped -= OnPlatformSkipped;
		}

		private void OnPlayerDeath()
		{
			State = GameState.Dead;
			Time.timeScale = 0f;
		}

		// Score 1 point when the player hops.
		private void OnPlatformTouched(Platform _) => Score++;

		// Score 10 points when the player skips a platform.
		private void OnPlatformSkipped() => Score += 10;
	}
}
