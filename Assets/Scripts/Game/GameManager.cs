namespace Game
{
	using System.Collections;
	using Platforms;
	using Player;
	using Settings;
	using UI;
	using UnityEngine;
	using UnityEngine.SceneManagement;

	[DefaultExecutionOrder(-1)]
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance { get; private set; }
		public int Score { get; private set; }
		[field: SerializeField] public ProgressionSettings ProgressionSettings { get; private set; }
		public PlatformManager PlatformManager { get; private set; }

		[SerializeField] private UIImageFader imageFader;

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

			Time.timeScale = 1f;
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
			StartCoroutine(DeathCoroutine());
		}

		private IEnumerator DeathCoroutine()
		{
			Time.timeScale = .5f;
			
			yield return imageFader.FadeTo(1f, .5f);
			
			Time.timeScale = 0f;

			var currentHighScore = PlayerPrefs.GetInt(SettingsStrings.HighScore, 0);
			if (Score > currentHighScore) 
				PlayerPrefs.SetInt(SettingsStrings.HighScore, Score);
			
			PlayerPrefs.SetInt(SettingsStrings.LastScore, Score);
			PlayerPrefs.Save();

			yield return new WaitForSecondsRealtime(0.2f);
			
			Time.timeScale = 1f;
			
			// Return to main menu.
			SceneManager.LoadScene(0);
		}

		// Score 1 point when the player hops.
		private void OnPlatformTouched(Platform _) => Score++;

		// Score 10 (1+9) points when the player skips a platform.
		private void OnPlatformSkipped() => Score += 9;
	}
}
